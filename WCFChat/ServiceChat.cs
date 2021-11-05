using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace WCFChat
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "ServiceChat" в коде и файле конфигурации.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServiceChat : IServiceChat
    {
        private List<ServerUser> users = new List<ServerUser>();
        private int nextId = 1;
        public int Connect(string name)
        {
            ServerUser user = new ServerUser
            {
                ID = nextId,
                Name = name,
                OperationContext = OperationContext.Current
            };

            nextId++;

            SendMessage($"{user.Name} подключился к чату", 0);
            users.Add(user);

            return user.ID;
        }

        public void Disconnect(int id)
        {
            var user = users.FirstOrDefault(u => u.ID == id);
            if (user != null)
            {
                users.Remove(user);
                SendMessage($"{user.Name} вышел из чата", 0);
            }
        }

        public void SendMessage(string msg, int id)
        {
            foreach (var user in users)
            {
                string answer = DateTime.Now.ToShortTimeString() + " ";

                var currentUser = users.FirstOrDefault(u => u.ID == id);
                if (currentUser != null)
                {
                    answer += $" : {currentUser.Name} ";
                }

                answer += msg;

                user.OperationContext.GetCallbackChannel<IServerChatCallback>().MsgCallback(answer);
            }
        }
    }
}
