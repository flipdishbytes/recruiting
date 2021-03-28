using Flipdish.Recruiting.WebhookReceiver.Service;
using System.Threading.Tasks;

namespace Flipdish.Recruiting.WebhookReceiver.StateMachine
{
	public abstract class SendOrderEmailWorkFlowState
    {
        protected SendOrderEmailWorkflow _sendOrderEmailWorkflow;
        protected IFileService FileService { get; set; }
        public string EmailOrder { get; protected set; }


        public void SetContext(SendOrderEmailWorkflow sendOrderEmailWorkflow)
        {
            _sendOrderEmailWorkflow = sendOrderEmailWorkflow;
        }

        public abstract void Handle();
        public abstract Task HandleAsync();
    }
}

