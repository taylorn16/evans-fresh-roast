# Endpoints

All routes prefixed by: `/api/v1`

`POST /events`: create a new coffee roasting event
- Request body should include event details

`GET /events?page=x`: get all the existing roasting events, paginated
- Only returns event summary, not all details

`GET /events/{id}`: get an existing coffee roasting event
- Returns the associated coffees and orders with their invoices

`PUT /events/{id}`: update an existing coffee roasting event
- Can update the following fields (all are be optional):
    - name
    - isActive
    - roast date
    - order-by date
    - name
  
`POST /events/{id}/text-blast`: send a text blast for an event

`POST /events/{id}/reminder-text-blast`: send a follow-up text blast for an event
    
`PUT /events/{id}/coffees`: add coffees to a roasting event
- Request body should include the coffee ids

`DELETE /events/{id}/coffees`: remove coffees from a roasting event
- Request body should include the coffee ids

`PUT /events/{id}/contacts`: add contacts to a roasting event
- Request body should include the contact ids

`DELETE /events/{id}/contacts`: remove contacts from a roasting event
- Request body should include the contact ids

`POST /coffees`: create a new available coffee selection
- Request body should include name, description, price, bag weight in oz

`GET /coffees`: get all available coffee selections

`PUT /coffees/{id}`: update a coffee
- update name, description, price, or weight per bag (oz) - all fields optional

`POST /contacts`: add a new contact
- Request body should include name and number 

`GET /contacts`: get all available contacts

`PUT /contacts/{id}`: update a contact
- update name; can't update number

`PUT /events/{event_id}/orders/{order_id}/invoices/{invoice_id}`: update an invoice
- Can only mark the invoice as paid and include a payment method

`POST /auth/login`: login; pretty simple
- Must supply login creds
- Returns a JWT

`POST /sms`: receive incoming sms messages from the vonage API webhook
    