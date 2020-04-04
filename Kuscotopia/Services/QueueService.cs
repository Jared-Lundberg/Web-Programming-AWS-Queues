using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Kuscotopia.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kuscotopia.Services
{
    public class QueueService
    {
        private BasicAWSCredentials credentials;
        private AmazonSQSClient amazonSQSClient;

        private static string QueueUrl = "https://sqs.us-east-2.amazonaws.com/619273675501/WebProject8";

        private MessageEntity messageEntity;

        public Random rnd;

        public QueueService(MessageEntity messageEntity, Random rnd)
        {
            credentials = new BasicAWSCredentials("AKIAIKVQOGATY7ME2NZQ", "ooNovy5whj2Kt/nyyKbwChSqYZAJ7cWEPM6WVOTF");
            amazonSQSClient = new AmazonSQSClient(credentials, RegionEndpoint.USEast2);
            this.rnd = rnd;
            this.messageEntity = messageEntity;
        }

        public async Task QueueWorkAsync(int workCount) {
            while (workCount > 0)
            {
                string[] option = { "Carry", "Build", "Survey" };
                int type = rnd.Next(0, 3);
                messageEntity.Type = option[type];
                if (type == 0)
                {
                    messageEntity.Message = "The peasant is carrying bricks!";
                }
                if (type == 1)
                {
                    messageEntity.Message = "The peasant is building a bridge";
                    messageEntity.Data = rnd.Next(1, 5);
                }
                if (type == 2)
                {
                    messageEntity.Message = "The other peasants r gr8!";
                    messageEntity.Data = rnd.Next(500, 1000);
                }
                var sendMessageRequest = new SendMessageRequest()
                {
                    QueueUrl = QueueUrl,
                    MessageBody = JsonConvert.SerializeObject(messageEntity)
                };

                await amazonSQSClient.SendMessageAsync(sendMessageRequest);
                workCount--;
            }
        }
    }
}
