using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace Obsidize.BehaviourTrees.Editor
{

    public class BehaviourTreeGraphView : GraphView
    {

        private static readonly Vector2 duplicationOffset = Vector2.one * 20f;
        private static readonly bool verbose = false;

        public new class UxmlFactory : UxmlFactory<BehaviourTreeGraphView, UxmlTraits> { }

        public Action<BehaviourTreeNodeView> OnNodeSelected;

        private BehaviourTree tree;

        public BehaviourTreeGraphView()
		{

            Insert(0, new GridBackground());
            AddGraphManipulators();

            styleSheets.Add(BehaviourTreeEditorUtility.Settings.RootStyleSheet);
            Undo.undoRedoPerformed += OnUndoRedoPerformed;

            serializeGraphElements += CutOrCopyOperation;
            unserializeAndPaste += PasteOperation;
            canPasteSerializedData += CanPaste;
        }

        private void RepopulateView()
		{
            PopulateView(tree);
        }

        private void OnUndoRedoPerformed()
        {
            RepopulateView();
            AssetDatabase.SaveAssets();
        }

        private bool CanPaste(string data)
        {
            return true;
        }

        private string CutOrCopyOperation(IEnumerable<GraphElement> elements)
        {
            return elements
                .Select(e => e as BehaviourTreeNodeView)
                .Where(n => n != null)
                .Select(e => e.Target)
                .ToSerializedNodeList();
        }

        private void PasteOperation(string operationName, string data)
		{

            var nodes = NodeSerializationUtility.DeserializeAllNodes(data);

            foreach (var node in nodes)
			{
                node.GraphPosition += duplicationOffset;
                tree.CreateNodeAssetFromInstance(node);
            }

            RepopulateView();
        }

        private void AddGraphManipulators()
		{
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
        }

		public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
		{
           return ports.Where(endPort => CanConnectPorts(startPort, endPort)).ToList();
		}

        public void ClearNodesAndConnections()
		{
            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;
        }

        private bool CanConnectPorts(Port startPort, Port endPort)
		{
            return startPort.direction != endPort.direction
                && startPort.node != endPort.node;
		}

		internal void PopulateView(BehaviourTree tree)
		{
            
            if (verbose) Debug.Log("PopulateView() -> " + tree);
            this.tree = tree;

            ClearNodesAndConnections();

            tree.CreateRootNodeAssetIfNeeded();
            tree.Children.ForEach(CreateNodeView);
            tree.Children.ForEach(CreateNodeViewConnections);

            FrameAll();
        }

        private BehaviourTreeNodeView FindNodeView(Node node)
		{
            return (node != null) 
                ? GetNodeByGuid(node.Guid) as BehaviourTreeNodeView
                : null;
		}

        private bool HasTreeRef(string actionName)
		{

            if (tree == null)
			{
                Debug.LogWarning($"Cannot perform action '{actionName}' on null tree reference");
                return false;
			}

            return true;
		}

		public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
		{

            var targetPosition = viewTransform.matrix.inverse.MultiplyPoint(evt.localMousePosition);

            AddContextMenuNodeCategory<DecoratorNode>(evt, targetPosition);
            AddContextMenuNodeCategory<CompositeNode>(evt, targetPosition);
            AddContextMenuNodeCategory<ConditionNode>(evt, targetPosition);
            AddContextMenuNodeCategory<ActionNode>(evt, targetPosition);
        }

        private void AddContextMenuNodeCategory<T>(ContextualMenuPopulateEvent evt, Vector3 targetPosition) where T : Node
		{

            var types = TypeCache.GetTypesDerivedFrom<T>().Where(type => !type.IsGenericType && !type.IsAbstract);
            var categoryName = typeof(T).Name;
            var actionNamePrefix = categoryName + "s/";

            foreach (var type in types)
            {
                var actionName = actionNamePrefix + type.Name;
                evt.menu.AppendAction(actionName, action => CreateNodeAsset(action, type, targetPosition));
            }
        }

        public void SyncAllNodeStates()
		{
            ForEachNodeView(n => n.SyncWithNodeState());
        }

        private void SortAllCompositeNodeOrders()
        {
            ForEachNodeView(n => n.SortChildrenByPosition());
        }

        private void ForEachNodeView(Action<BehaviourTreeNodeView> action)
		{
            foreach (var nodeView in nodes.Select(n => n as BehaviourTreeNodeView).Where(n => n != null))
            {
                action(nodeView);
            }
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {

            if (!HasTreeRef(nameof(OnGraphViewChanged)))
			{
                return graphViewChange;
            }

            graphViewChange.elementsToRemove?.RemoveAll(ShouldPreventGraphElementDeletion);
            graphViewChange.elementsToRemove?.ForEach(HandleDeletedGraphElement);
            graphViewChange.edgesToCreate?.ForEach(HandleCreatedGraphEdge);

            if (graphViewChange.movedElements != null)
			{
                SortAllCompositeNodeOrders();
			}

            return graphViewChange;
        }

		private void CreateNodeView(Node node)
        {
            var nodeView = new BehaviourTreeNodeView(node);
            nodeView.OnNodeSelected = OnNodeSelected;
            AddElement(nodeView);
        }

        private Node CreateNodeAsset(DropdownMenuAction action, Type type, Vector3 targetPosition)
        {

            if (!HasTreeRef(nameof(CreateNodeAsset)))
            {
                return null;
            }

            var node = tree.CreateNodeAsset(type);
            node.GraphPosition = targetPosition;
            CreateNodeView(node);

            return node;
        }

        private bool ShouldPreventGraphElementDeletion(VisualElement element)
		{
            var nodeView = element as BehaviourTreeNodeView;

            if (nodeView != null)
			{
                return nodeView.Target is RootNode;
            }

            return false;
        }

        private void HandleDeletedGraphElement(GraphElement element)
        {

            var nodeView = element as BehaviourTreeNodeView;

            if (nodeView != null)
            {
                TryDeleteNodeViewFromGraph(nodeView);
                return;
            }

            var edge = element as Edge;

            if (edge != null)
            {
                DeleteEdgeFromGraph(edge);
            }
        }

        private void HandleCreatedGraphEdge(Edge edge)
        {

            var childView = edge.input.node as BehaviourTreeNodeView;

            if (childView == null)
			{
                return;
			}

            var parentView = edge.output.node as BehaviourTreeNodeView;

            if (parentView == null)
			{
                return;
			}

            var parent = parentView.Target;
            var child = childView.Target;

            Undo.RecordObject(parent, "Behaviour Tree (Add Child)");
            parent.AttachChild(child);
            EditorUtility.SetDirty(parent);
		}

        private void DeleteEdgeFromGraph(Edge edge)
		{

            var childView = edge.input.node as BehaviourTreeNodeView;

            if (childView == null)
            {
                return;
            }

            var parentView = edge.output.node as BehaviourTreeNodeView;

            if (parentView == null)
            {
                return;
            }

            var parent = parentView.Target;
            var child = childView.Target;

            Undo.RecordObject(parent, "Behaviour Tree (Remove Child Edge)");
            parent.DetachChild(child);
            EditorUtility.SetDirty(parent);
        }

        private void TryDeleteNodeViewFromGraph(BehaviourTreeNodeView nodeView)
		{

            var target = nodeView.Target;

            if (target is RootNode)
			{
                Debug.LogWarning("Cannot delete root node from Behaviour Tree");
                return;
			}

            tree.DeleteNodeAsset(target);
        }

        private void CreateNodeViewConnections(Node parent)
        {

            if (parent == null)
            {
                return;
            }

            if (!HasTreeRef(nameof(CreateNodeViewConnections)))
            {
                return;
            }

            var children = parent.GetChildren();

            if (children == null)
            {
                return;
            }

            var parentView = FindNodeView(parent);

            if (parentView == null || parentView.Output == null)
            {
                return;
            }

            foreach (var child in children)
            {

                if (child == null)
                {
                    continue;
                }

                var childView = FindNodeView(child);

                if (childView == null || childView.Input == null)
                {
                    continue;
                }

                var edge = parentView.Output.ConnectTo(childView.Input);
                var edgeControl = edge.edgeControl;
                
                edgeControl.AddToClassList("control");
                childView.parentEdge = edge;

                AddElement(edge);
            }
        }
    }
}
