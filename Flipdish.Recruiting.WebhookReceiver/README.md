# recruiting

Function of the application: Receive a Flipdish order.created webhook, transform into an email and use the menu elements metadata to read and display barcodes.

Test at local environment:
POST the TestWebhooks\1.json files body to https://{localhost}/api/WebhookReceiver?to=john.doe@mail.com&to=jane.doe@mail.com&currency=eur&metadatakey=eancode&storeId=1234&storeId=12345
parameters:
- to: email address where to send the mail (can add multiple)
- currency: currency to be displayed in the mail
- metadatakey: metadata key for the EAN code (case sensitive)
- storeid: store identifiers control which stores orders we want to process (can add multiple)