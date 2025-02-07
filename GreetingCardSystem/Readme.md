# Greeting Card Messaging System

This project implements a serverless greeting card messaging system using AWS services, where one user can send a greeting card to another. The system consists of:

- **WebAPI** (REST API) to send messages
- **Lambda 1** (Producer) to send messages to an SQS queue
- **AWS SQS** (Message Queue) to store messages temporarily
- **Lambda 2** (Consumer) to process SQS messages and store them in DynamoDB
- **DynamoDB** to store and retrieve greeting cards

## Architecture Workflow

1. **User1 sends a greeting card** via WebAPI.
2. **WebAPI calls Lambda 1**, which pushes the message to AWS SQS.
3. **Lambda 2 listens to SQS**, retrieves the message, and stores it in DynamoDB.
4. **User2 retrieves the message** from DynamoDB using WebAPI.

## AWS Services Used

- **AWS Lambda** (Python-based)
- **AWS SQS** (Queue for message handling)
- **AWS DynamoDB** (NoSQL database for storage)
- **AWS IAM Roles & Policies** (Permissions for Lambda to access SQS and DynamoDB)

---

## 1️⃣ Setting Up AWS Resources

### **Step 1: Create an SQS Queue**
1. Go to AWS Console → SQS → Create Queue.
2. Choose **Standard Queue**.
3. Name it `GreetingMessageQueue`.
4. Note down the **Queue URL**.

### **Step 2: Create a DynamoDB Table**
1. Go to AWS Console → DynamoDB → Create Table.
2. Set **Table Name**: `GreetingCards`
3. Set **Primary Key**:
   - **Partition Key**: `To` (String) - Receiver's ID
   - **Sort Key**: `CreatedAt` (String) - Timestamp
4. Enable On-Demand Capacity Mode.

### **Step 3: Create IAM Role & Policy**
1. Go to AWS Console → IAM → Roles → Create Role.
2. Select **AWS Lambda** as trusted entity.
3. Attach policies:
   - **AmazonSQSFullAccess** (to send and receive messages)
   - **AmazonDynamoDBFullAccess** (to write messages to DynamoDB)
   - Custom policy (if needed) with the correct ARN for security.
4. Attach this role to both Lambda functions.

---

## 2️⃣ Implementing AWS Lambda Functions (Python)

### **Lambda 1 (Producer) - Sends Message to SQS**

```python
import json
import boto3
import os

sqs = boto3.client('sqs')
QUEUE_URL = os.getenv("QUEUE_URL", "<YOUR_SQS_QUEUE_URL>")

def lambda_handler(event, context):
    try:
        body = json.loads(event['body'])
        
        if not all(k in body for k in ("From", "To", "Message", "CreatedAt")):
            return {"statusCode": 400, "body": "Invalid input data"}
        
        # Send message to SQS
        sqs.send_message(QueueUrl=QUEUE_URL, MessageBody=json.dumps(body))
        
        return {"statusCode": 200, "body": "Greeting card sent successfully!"}
    except Exception as e:
        return {"statusCode": 500, "body": str(e)}
```

#### **Deployment Steps for Lambda 1:**
- Create a new Lambda function in AWS Console.
- Choose **Python 3.x** as runtime.
- Paste the above code.
- Set **QUEUE_URL** as an environment variable.
- Assign the IAM role with **SQS sendMessage permissions**.

---

### **Lambda 2 (Consumer) - Reads SQS & Stores in DynamoDB**

```python
import json
import boto3
import os

dynamodb = boto3.resource('dynamodb')
table = dynamodb.Table("GreetingCards")

def lambda_handler(event, context):
    try:
        for record in event['Records']:
            message = json.loads(record['body'])
            
            # Store in DynamoDB
            table.put_item(Item=message)
            
        return {"statusCode": 200, "body": "Message processed successfully!"}
    except Exception as e:
        return {"statusCode": 500, "body": str(e)}
```

#### **Deployment Steps for Lambda 2:**
- Create a new Lambda function.
- Choose **Python 3.x** as runtime.
- Paste the above code.
- Attach an IAM role with **SQS receiveMessage & DynamoDB putItem permissions**.
- Configure SQS as **Trigger** for this Lambda.

---

## 3️⃣ WebAPI Implementation

### **.NET 8 WebAPI (C#) - Sending Messages to Lambda 1**

```csharp
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class GreetingController : ControllerBase
{
    private readonly IHttpClientFactory _clientFactory;

    public GreetingController(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    [HttpPost("sendMessage")]
    public async Task<IActionResult> SendMessage([FromBody] GreetingCard greetingCard)
    {
        if (greetingCard == null || string.IsNullOrEmpty(greetingCard.Message) ||
            string.IsNullOrEmpty(greetingCard.To) || string.IsNullOrEmpty(greetingCard.From))
            return BadRequest("Invalid greeting card data.");

        var client = _clientFactory.CreateClient();
        var json = JsonConvert.SerializeObject(greetingCard);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PostAsync("<LAMBDA_1_URL>", content);

        return response.IsSuccessStatusCode ? Ok("Greeting card sent successfully.") : StatusCode(500, "Error sending greeting card.");
    }
}

public class GreetingCard
{
    public string From { get; set; }
    public string To { get; set; }
    public string Message { get; set; }
    public string CreatedAt { get; set; }
}
```

---

## 4️⃣ Testing the System

### **Test Lambda 1 (Send Message to SQS)**
Send a POST request to Lambda 1 with:

```json
{
  "From": "User1",
  "To": "User2",
  "Message": "Happy Birthday!",
  "CreatedAt": "2025-02-07T12:00:00Z"
}
```

### **Check Messages in SQS**
1. Go to AWS Console → SQS → `GreetingMessageQueue`
2. Click **Send and Receive Messages** → Poll for messages.

### **Test Lambda 2 (Process Message & Store in DynamoDB)**
1. Manually invoke Lambda 2.
2. Check DynamoDB Table for the new entry.

### **Test WebAPI**
1. Run the .NET WebAPI project.
2. Use **Postman** or **cURL** to send a POST request to:
   ```
   POST http://localhost:5000/api/greeting/sendMessage
   ```
3. Verify message flow from WebAPI → Lambda 1 → SQS → Lambda 2 → DynamoDB.

---

## 🎉 Conclusion
This serverless architecture ensures reliable message delivery with AWS Lambda, SQS, and DynamoDB. Let me know if you need modifications! 🚀

