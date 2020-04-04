using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Worker.Entities;

namespace Worker
{
    class Program
    {
        private static BasicAWSCredentials credentials;
        private static AmazonSQSClient amazonSQSClient;

        private static string QueueUrl = "QueueURL";

        static void Main(string[] args)
        {
            Console.WriteLine("Starting to read messages...");

            credentials = new BasicAWSCredentials("credentials", "credentials");
            amazonSQSClient = new AmazonSQSClient(credentials, RegionEndpoint.USEast2);

            ReadMessagesAsync().Wait();
        }

        public static async Task ReadMessagesAsync()
        {
            var request = new ReceiveMessageRequest();
            request.QueueUrl = QueueUrl;
            request.MaxNumberOfMessages = 10;
            request.WaitTimeSeconds = 10;

            while (true)
            {
                var messages = await amazonSQSClient.ReceiveMessageAsync(request);

                foreach (var message in messages.Messages)
                {
                    MessageEntity deserializedProduct = JsonConvert.DeserializeObject<MessageEntity>(message.Body);
                    if(deserializedProduct.Type == "Carry")
                    {
                        Console.WriteLine(deserializedProduct.Message);
                    }
                    if(deserializedProduct.Type == "Build")
                    {
                        if(deserializedProduct.Data > 0)
                        {
                            deserializedProduct.Data -= 1;
                            if(deserializedProduct.Data == 0)
                            {
                                Console.WriteLine("The Building is Done!");
                            }
                            else
                            {
                                Console.WriteLine("The Building has " + deserializedProduct.Data + " more steps");
                            }
                            var sendMessageRequest = new SendMessageRequest()
                            {
                                QueueUrl = QueueUrl,
                                MessageBody = JsonConvert.SerializeObject(deserializedProduct)
                            };

                            await amazonSQSClient.SendMessageAsync(sendMessageRequest);
                        }
                        else
                        {
                            Console.WriteLine("The Building is Done!");
                        }
                    }
                    if(deserializedProduct.Type == "Survey")
                    {
                        Console.WriteLine("Starting Survey");
                        await Task.Delay(deserializedProduct.Data);
                        Console.WriteLine("Survey Finished");
                    }
                    // for assignment 8, you'll need to do a JSON convert on the body


                    var deleteTask = amazonSQSClient.DeleteMessageAsync(new DeleteMessageRequest()
                    {
                        QueueUrl = QueueUrl,
                        ReceiptHandle = message.ReceiptHandle
                    });
                }
            }
        }
    }
}
