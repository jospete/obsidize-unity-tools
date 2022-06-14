using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Obsidize.BehaviourTrees.Editor
{
    public class BehaviourTreeNodeView : UnityEditor.Experimental.GraphView.Node
    {

		private const string activeEdgeClassName = "bt-active-path";

		public Action<BehaviourTreeNodeView> OnNodeSelected;
		public Edge parentEdge;

		private Node _target;
		private Port _input;
		private Port _output;
		private Label _descriptionLabel;

        public Node Target => _target;
		public Port Input => _input;
		public Port Output => _output;

        public BehaviourTreeNodeView(Node node) : base(BehaviourTreeEditorUtility.Settings.NodeViewUxmlPath)
		{

            _target = node;
			_descriptionLabel = this.Q<Label>("description-label");

			viewDataKey = node.Guid;
			style.left = node.GraphPosition.x;
			style.top = node.GraphPosition.y;
			styleSheets.Add(BehaviourTreeEditorUtility.Settings.NodeViewStyleSheet);

			SyncNodeTextContent();
			CreateInputPorts();
			CreateOutputPorts();
			SetupClasses();
		}

		private void SyncNodeTextContent()
		{
			title = ObjectNames.NicifyVariableName(Target.DisplayName);
			_descriptionLabel.text = Target.Description;
		}

		private void SetupClasses()
		{
			AddToClassList(Target.PrimaryUssClass);
		}

		public override void SetPosition(Rect newPos)
		{
			base.SetPosition(newPos);
			Undo.RecordObject(_target, "Behaviour Tree (Set Position)");
			Target.GraphPosition = newPos.min;
			EditorUtility.SetDirty(_target);
		}

		public override void OnSelected()
		{
			base.OnSelected();
			OnNodeSelected?.Invoke(this);
		}

		public void SortChildrenByPosition()
		{

			var composite = Target as CompositeNode;

			if (composite != null)
			{
				composite.SortChildrenByGraphPositionHorizontal();
			}
		}

		private void AddActivePathClass(VisualElement element)
		{
			if (element != null)
			{
				element.AddToClassList(activeEdgeClassName);
			}
		}

		private void RemoveActivePathClass(VisualElement element)
		{
			if (element != null)
			{
				element.RemoveFromClassList(activeEdgeClassName);
			}
		}

		public void SyncWithNodeState()
		{

			RemoveActivePathClass(parentEdge);
			RemoveActivePathClass(Input);
			RemoveActivePathClass(Output);
			SyncNodeTextContent();

			foreach (var state in BehaviourTreeEditorUtility.ALL_NODE_STATES)
			{
				RemoveFromClassList(state.GetGraphViewClassName());
			}

			if (!Application.isPlaying || Target == null || Target.Idle)
			{
				return;
			}

			AddToClassList(Target.State.GetGraphViewClassName());

			if (Target.State == NodeState.Running)
			{
				AddActivePathClass(parentEdge);
				AddActivePathClass(Input);
				AddActivePathClass(Output);
			}
		}

		private void NormalizePortLayout(Port port)
		{

			if (port == null)
			{
				return;
			}

			port.Remove(port.Q<Label>());
			port.AddToClassList("bt-node-port");

			var connector = port.Q("connector");

			// for some ungodly reason the picking mode on the main area
			// of the port defaults to "ignore", which makes the port useless.
			if (connector != null)
			{
				connector.pickingMode = PickingMode.Position;
			}
		}

		private void CreateInputPorts()
		{

			if (Target is RootNode)
			{
				return;
			}

			_input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));

			if (_input == null)
			{
				return;
			}

			NormalizePortLayout(_input);
			inputContainer.Add(_input);
		}

		private void CreateOutputPorts()
		{

			if (Target.ChildCapacity == NodeChildCapacity.None)
			{
				return;
			}

			var portCapacity = Target.ChildCapacity == NodeChildCapacity.Multi
				? Port.Capacity.Multi
				: Port.Capacity.Single;

			_output = InstantiatePort(Orientation.Vertical, Direction.Output, portCapacity, typeof(bool));

			if (_output == null)
			{
				return;
			}

			NormalizePortLayout(_output);
			outputContainer.Add(_output);
		}
	}
}
