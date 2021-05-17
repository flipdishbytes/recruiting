We are happy you're interested in an engineering position at Flipdish!

### About This Challenge
We want to see how you construct software, but we also know that you're busy.

It doesn't matter how long you spend on it! You could spend 5 minutes, or 5 hours -- the amount of time is totally up to you. It'll really help us understand your perspectives on software when we see how you implement the solution.

### React Coding Challenge

Congratulations on making it to the coding evaluation in our recruitment
process and thank you for taking the time to participate. We realise you have
options and are excited that you are considering joining our team. At Flipdish
we use an example of your code as the basis for a technical discussion with
our engineers. This gives you the opportunity to present yourself in the best
light. We look forward to hearing and seeing how you solved the problem and
why you chose the solution you did.
Good luck!

#### Render Flipdish restaurant menu

Read the following description of the menu data object at Flipdish and create
a solution that renders the menu in the browser. To facilitate easier evaluation
by Flipdish engineers, our preference is for a JavaScript solution, but you can
use any programming language you choose. We also like to use ReactJS.

- The restaurant menu object contains sections (such as Starters, Curries
and Soft Drinks) and items within each section (such as Vegetable Spring
Rolls, Massaman Curry and Coca Cola). See the data structure explanation
below.
 - Choose a suitable layout and style to render your menu
 - At a minimum, your solution should display the menu item name and image.
Ideally it will show the description and price as well (if present).
 - Our engineers will invite you to discuss the technical aspects of your
solution via video call with screen sharing.

#### Data Structure

The Flipdish menu combines the products
(<em>MenuSectionItems</em> & <em>MenuOptionSetItems</em>) and layout in a single structure,
known as the <em>Menu</em>.

The Menu is made up of MenuSections which contain <em>MenuSectionItems</em>.
MenuSectionItems may be standalone or contain <em>MenuItemOptionSets</em>. Each
MenuItemOptionSet will contain one or more <em>MenuItemOptionSetItem</em>.

#### MenuItemOptionSets

A MenuItemOptionSet contains a boolean flag named <em>IsMasterOptionSet</em>. This
would be set to <em>true</em> for options that could be treated as standalone
products, as opposed to additions to a product (eg. "Ketchup") or a
modification to a product (eg. "large").
When <em>IsMasterOptionSet</em> is set to true on a MenuItemOptionSet, the price of the
menu item is calculated based on the MenuItemOptionSetItems in the
MenuItemOptionSet, and the price set on the MenuSectionItem is ignored.

#### Sample Menu...
https://menus.flipdish.co/prod/16798/e6220da2-c34a-4ea2-bb51-a3e190fc5f08.json


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
  - Email to let us know that the PR is ready for review by our team.

Good luck with the assignment, and have fun with it!
