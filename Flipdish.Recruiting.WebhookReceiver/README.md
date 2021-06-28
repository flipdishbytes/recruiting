We are happy you're interested in an engineering position at Flipdish!

### About This Challenge
We want to see how you construct software, but we also know that you're busy. So, rather than ask you to build something from scratch, we want you to take whatever time is reasonable and refactor some of the code in this application. Just make it better, in literally any way that you see fit.

It doesn't matter how long you spend on improving it! You could spend 5 minutes, or 5 hours -- the amount of time is totally up to you. It'll really help us understand your perspectives on software when we see how you improve existing code that you haven't encountered before. Feel free to refactor, change, improve, or optimize any part of the application at all.

### About The Application
The application sets up a webhook which listens for order creation messages. Once a message is received on that webhook, it transforms it into an email that includes barcode images for items that have the code defined as metadata.

#### Testing the application 
In your local environment, simply POST the contents of `TestWebhooks\1.json` to `https://{localhost}/api/WebhookReceiver?to=john.doe@mail.com&to=jane.doe@mail.com&currency=eur&metadatakey=eancode&storeId=1234&storeId=12345`.

The parameters are as follows:
- `to` - email address where to send the mail (can add multiple)
- `currency` - currency to be displayed in the mail
- `metadatakey` - metadata key for the EAN code (case sensitive)
- `storeid` - store identifiers control which stores orders we want to process (can add multiple)

### Also...
* The application is deliberately written to be "not very clean code". It should not be viewed as representative of acceptable code at Flipdish (it isn't).
* This README is deliberately sparse, so you'll have to read the code to truly understand what the application does.
* There is no SMTP service configured in the application code. If you would like to set one up yourself to see the email get sent, feel free to do so. If you don't have an SMTP service configured, an HTML email will be created locally (although it won't get delivered anywhere).
* Please keep track of the amount of time you do spend working on cleaning this code up. We'll want to know this when we review your solution, because it'll help us set our expectations appropriately.

### Submitting your work
* We work each day in GitHub, so we want to review your code in GitHub too. However, please do NOT submit a Pull Request to this repository because it will become visible for other candidates (or even your current employer) to see your work.
* Instead, please do the following:
  - Create a [private repository](https://docs.github.com/en/free-pro-team@latest/github/creating-cloning-and-archiving-repositories/about-repository-visibility) under your own GitHub account.
  - Commit all of the code from this repository to the `master`branch in your new private repository as a starting point.
  - Create a new branch in your private repository.
  - Commit your changes and refactoring work to the new branch.
  - Create a [Pull Request](https://docs.github.com/en/free-pro-team@latest/github/collaborating-with-issues-and-pull-requests/about-pull-requests) based on your branch.
  - Include a description of what you changed in the Pull Request, and how long you spent on the work. 
  - Invite the GitHub user `flipdish-reviewers` to be a [collaborator](https://docs.github.com/en/free-pro-team@latest/github/setting-up-and-managing-your-github-user-account/inviting-collaborators-to-a-personal-repository) on your private repository.
  - Email us with a link to your PR to let us know that it is ready for review by our team.
  
Good luck with the assignment, and have fun with it!
