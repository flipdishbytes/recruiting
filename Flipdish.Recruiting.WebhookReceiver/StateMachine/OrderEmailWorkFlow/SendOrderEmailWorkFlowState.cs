using Flipdish.Recruiting.WebhookReceiver.Service;
using System.Threading.Tasks;

namespace Flipdish.Recruiting.WebhookReceiver.StateMachine
{
	public abstract class SendOrderEmailWorkFlowState
    {
        protected SendOrderEmailWorkflow SendOrderEmailWorkflow;

        public void SetContext(SendOrderEmailWorkflow sendOrderEmailWorkflow)
        {
            SendOrderEmailWorkflow = sendOrderEmailWorkflow;
        }

        public abstract void Handle();
        public abstract Task HandleAsync();
    }
}

