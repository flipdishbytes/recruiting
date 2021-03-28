namespace Flipdish.Recruiting.WebhookReceiver.StateMachine
{
	public class SendOrderEmailWorkflow
    {
        public SendOrderEmailWorkFlowState WorkFlowState { get; private set; }

        public void TransitionTo(SendOrderEmailWorkFlowState sendOrderEmailWorkFlowState)
        {
            WorkFlowState = sendOrderEmailWorkFlowState;
            WorkFlowState.SetContext(this);
        }

        public void Continue()
        {
            WorkFlowState.Handle();
        }  
        
        public void ContinueAsync()
        {
            WorkFlowState.HandleAsync();
        }
    }
}
