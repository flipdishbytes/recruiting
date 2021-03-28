using System;

namespace Flipdish.Recruiting.WebhookReceiver.StateMachine
{
	public class SendOrderEmailWorkflow
    {
        public SendOrderEmailWorkFlowState SendOrderEmailWorkFlowState { get; private set; }

        public SendOrderEmailWorkflow(SendOrderEmailWorkFlowState sendOrderEmailWorkFlowState)
        {
            TransitionTo(sendOrderEmailWorkFlowState);
        }

        public void TransitionTo(SendOrderEmailWorkFlowState sendOrderEmailWorkFlowState)
        {
            SendOrderEmailWorkFlowState = sendOrderEmailWorkFlowState;
            SendOrderEmailWorkFlowState.SetContext(this);
        }

        public void ConstructMail()
        {
            SendOrderEmailWorkFlowState.Handle();
        }  
        
        public void SendAsync()
        {
            SendOrderEmailWorkFlowState.HandleAsync();
        }
    }
}
