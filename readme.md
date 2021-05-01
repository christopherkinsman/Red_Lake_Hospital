# Red Lake Hospital Project - Team 6

- Admin Account: Usr: admin@admin.com Pass: Test123*
- Regular User Account: Usr: user@user.com Pass: Test123*


## Nai-Hsien's Contributions - Online donation feature
### Models/ViewModels 
Models/Donation.cs <br />
Models/Donor.cs <br />
ViewModels/ShowDonation.cs
ViewModels/UpdateDonation.cs
### Controllers
Controllers/DonationDataController.cs <br />
Controllers/DonationController.cs <br />
Controllers/DonorDataController.cs <br />
### Views
all the views under donation and donor folders. All CRUD features are functioning now except for the following:
Views/Donation/Edit.cshtml (not currently fully functioning) <br />

### Debugging:
1. Solving merging conflict <br />
    - https://docs.github.com/en/github/collaborating-with-issues-and-pull-requests/resolving-a-merge-conflict-using-the-command-line#removed-file-merge-conflicts
    - see also project folder: documentation.pdf
2. Understanding how to work with git.ignore <br />
    - https://docs.microsoft.com/en-us/azure/devops/repos/git/ignore-files?view=azure-devops&tabs=visual-studio#ignore-files-only-on-your-system

## Gord's Contributions - Job Posting and Job Application Features

### Changes for Final Submission
- Implemented public-private key security for API controller resources
- Created new API controller to distribute server's public key to clients
- Split up JobsApiController by subject entities into JobsApplicationsApiController and JobPostingsApiController
- Moved job posting list view's processing work to the API controller using option flags allowing the return of all job postings or only unexpired/current job postings

#### Files Changed or Added for Final Submission
- /Controllers/JobsViewController.cs
- /Controllers/JobsApplicationsApiController.cs
- /Controller/JobsPostingsApiController.cs
- /Controller/PublicKeyDistributionController.cs
- /Models/Authentication.cs
- /Models/PublicKeyDto.cs

### Controllers
Set up role and users <br />
JobsApiController.cs <br />
JobsViewController.cs <br />

### Models and ViewModels
DepartmentsModel.cs <br />
JobApplicationsModel.cs <br />
JobPostingsModel.cs <br />
JobApplicationViewModel.cs <br />
JobPostingsViewModel.cs <br />

### Views
All views inside /Views/JobView directory


## Kunal's Contributions - Contact Us and Send E-cards Features

### Changes for final Submission 
- Proper Commenting Added.
- Added responsiveness to the features.
- Customized HTML/CSS
- Styling & Adding dynamic content to the home page (Shared/_Layout.cshtml)
- Added adminstrator log in functionality to operate CRUD on admin side.

### Controllers
ContactsController.cs <br />
ContactsDataController.cs <br />
EcardsController.cs <br />
EcardsDataController.cs <br />

### Models and ViewModels
Contacts.cs <br />
Ecards.cs <br />
ShowContact.cs <br />
UpdateContact.cs <br />
ListContact.cs <br />
ShowEcard.cs <br />
EditEcard.cs <br />

### Views
## Views for Contact Us
All views inside /Views/Contact directory
## Views for E-cards
All views inside /Views/Ecards directory

## Jerrin's Contributions - Department and Testimonial Features

### Controllers

TestimonialDataController.cs <br />
TestimonialController <br />
DepartmentDataController.cs <br />
DepartmentController.cs <br />

### Models and ViewModels

ViewModels - DepartmentDetails.cs, TestimonialDetails.cs
Models - Testimonial.cs, DepartmentsModel.cs

### Views
## Views for Department
All views inside /Views/Department directory
## Views for Testimonial
All views inside /Views/Testimonial directory


## Braydons's Contributions - FAQ and Photos

### Controllers
FaqDataController.cs <br />
FaqController.cs <br />
PhotoDataController.cs <br />
PhotoController.cs <br />
DepartmentsDataController.cs

### Models and ViewModels
Faq.cs <br />
Phot.cs <br />
ListFaq.cs <br />
ListPhoto.cs <br />
ShowFaq.cs <br />
ShowPhoto.cs <br />
UpdatePhoto.cs <br />
UpdateFaq.cs <br />

### Views
## Views for Faq
All views inside /Views/Faq directory
## Views for Photos
All views inside /Views/Photo directory

## Chris's Contributions - Blog and Feedback Features

### Changes for Final Submission
- Added department option for the feedback feature
- Added user id to the blof feature

#### Files Changed or Added for Final Submission
- /Controllers/BlogController.cs
- /Controllers/FeedbackApiController.cs
- /Controller/FeedbackController.cs
- /Views/Blog/AdminList.cshtml
- /Views/Blog/AdminShow.cshtml
- /Views/Blog/Create.cshtml
- /Views/Blog/Show.cshtml
- /Views/Blog/Update.cshtml
- /Views/Feedback/Create.cshtml
- /Views/Feedback/Delete.cshtml
- /Views/Feedback/List.cshtml
- /Views/Feedback/Show.cshtml

### Controllers
BlogController.cs <br />
BlogApiController.cs <br />
FeedbackController.cs <br />
FeedbackApiController.cs <br />

### Models and ViewModels
BlogModel.cs <br />
ShowBlogViewModel.cs <br />
UpdateBlogViewModel.cs <br />
FeedbackModel.cs <br />
ShowFeedbackViewModel.cs <br />
UpdateFeedbackViewModel.cs <br />

### Views
All views inside /Views/Blog directory<br />
All views inside /Views/Feedback directory<br />
