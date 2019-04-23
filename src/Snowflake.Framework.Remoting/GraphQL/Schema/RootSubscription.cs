using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using GraphQL.Resolvers;
using GraphQL.Subscription;
using GraphQL.Types;

namespace Snowflake.Support.Remoting.GraphQL.RootProvider
{
    internal sealed class RootSubscription : ObjectGraphType<object>
    {
        List<Message> Numbers { get; }
        public RootSubscription()
        {
            this.Name = "Subscription";
            this.Description = "The subscription root of Snowflake's GraphQL interface";
            this.Numbers = new List<Message>();
            AddField(new EventStreamFieldType
            {
                Name = "exampleSubscription",
                Type = typeof(MessageType),
                Resolver = new FuncFieldResolver<Message, Message>(ResolveMessage),
                Subscriber = new EventStreamResolver<Message, Message>(Subscribe),
                
            });

            this.Numbers.Add(new Message() { Hello = "Hello World!", World = 0 });
            Task.Run( async () =>
            {
                while (true)
                {
                    await Task.Delay(1000);
                    Console.WriteLine("New Added!");
                    this.Numbers.Add(new Message() { Hello = "Hello World!", World = this.Numbers.Count });
                }
            }).ConfigureAwait(false);
        }

        public class Message
        {
            public string Hello { get; set; }
            public int World { get; set; }
        }
        public class MessageType : ObjectGraphType<Message>
        {
            public MessageType()
            {
                Field(o => o.Hello);
                Field(o => o.World);
            }
        }

        private Message ResolveMessage(ResolveFieldContext<Message> context)
        {
            return context.Source;
        }
        private IObservable<Message> Subscribe(ResolveEventStreamContext<Message> context)
        {
            return this.Numbers.ToObservable();
        }
    }
}