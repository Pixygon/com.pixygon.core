using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pixygon.Core {
    public class Notifications : MonoBehaviour {

        public static Notifications Instance;

        public List<Notification> notifications;
        public NewNotification newNotification;

        private void Awake() {
            if(Instance != null)
                Destroy(this);
            else {
                Instance = this;
                notifications = new List<Notification>();
            }
        }

        public void SendNotification(string notificationID, string notification, Sprite sprite) {
            var found = false;
            foreach (var n in notifications.Where(n => n.ID == notificationID)) {
                n.Message = notification;
                n.Number += 1;
                n.Icon = sprite;
                found = true;
                newNotification.Invoke(n);
            }

            if (found) return;
            var n2 = new Notification {
                ID = notificationID,
                Message = notification,
                Number = 1,
                Icon = sprite
            };
            notifications.Add(n2);
            newNotification.Invoke(n2);
        }

        public void ClearNotifications() {
            notifications = new List<Notification>();
        }

        public delegate void NewNotification(Notification n);
    }

    public class Notification {
        public string ID;
        public string Message;
        public int Number;
        public Sprite Icon;
    }
}