using Final_Project.Dal;
using Final_Project.Models;
using Final_Project.ViewModels;
using Microsoft.AspNet.SignalR.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
//using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Threading.Tasks;

namespace Final_Project.Hubs
{
    public class OrderHub : Hub
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly WoltDbContext _context;


        public OrderHub(UserManager<AppUser> userManager, WoltDbContext context, IHubContext<OrderHub> hubContext)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task SendMessageToAdmin(string text)
        {
            AppUser Resuser = await _userManager.FindByNameAsync(Context.User.Identity.Name);
            AppUser admin =  _userManager.Users.Where(u => u.Role == "SuperAdmin").FirstOrDefault()
                ;
            Message message = new Message
            {
                Text=text,
                AppUserId=Resuser.Id,
                ReciveUserId=admin.Id,
                Date=DateTime.Now
            };
            _context.Messages.Add(message);
            _context.SaveChanges();
            if (Resuser.Role=="Store")
            {
                     Store store = _context.Stores.FirstOrDefault(r => r.Id == Resuser.StoreId);
                    if (admin.ConnectionId!=null)
                {
                   await Clients.Client(admin.ConnectionId).SendAsync("recivemessage",new { 
                   id=message.Id,
                   text=text,
                   date=message.Date.ToString("HH:mm"),
                   image= store.Image,
                   
                   });;
                }
            }
            else
            {
                if (Resuser.Role == "Restaurant")
                {
                    Restuorant restuorant = _context.Restuorants.FirstOrDefault(r => r.Id == Resuser.RestuorantId);
                    if (admin.ConnectionId != null)
                    {
                        await Clients.Client(admin.ConnectionId).SendAsync("recivemessage", new
                        {
                            id = message.Id,
                            text = text,
                            date = message.Date.ToString("HH:mm"),
                            image = restuorant.Image,

                        }); ;
                    }
                }
            }

        }
        public async Task SendMessageToBussines(string text,string usser)
        {
            AppUser admin = await _userManager.FindByNameAsync(Context.User.Identity.Name);
            AppUser user = await _userManager.FindByIdAsync(usser);
                ;
           
            Message message = new Message
            {
                Text = text,
                AppUserId = admin.Id,
                ReciveUserId = user.Id,
                Date = DateTime.Now
            };
            _context.Messages.Add(message);
            _context.SaveChanges();

            if (user.ConnectionId != null)
            {
                await Clients.Client(user.ConnectionId).SendAsync("recivemessageinres", new
                {
                    id = message.Id,
                    text = text,
                    date = message.Date.ToString("HH:mm"),
                    adminid= admin.Id
                }); 
            }
        }
        public async Task OrderProduct(string restuorantid, string Isdelivery, string Adress, string IsCard)
        {
            bool IsDelivery = bool.Parse(Isdelivery);
            bool Iscard = bool.Parse(IsCard);
            int Restuorantid = int.Parse(restuorantid);

            Restuorant restuorant = _context.Restuorants.FirstOrDefault(r => r.Id == Restuorantid);
            AppUser user = await _userManager.FindByNameAsync(Context.User.Identity.Name);
            AppUser ResUser = await _userManager.FindByIdAsync(restuorant.AppUserId);
            List<BasketItem> basketitems = _context.BasketItems.Include(bi => bi.Product).Include(bi => bi.AppUser).Include(bi => bi.Restuorant).ThenInclude(r=>r.Campaign).Include(bi => bi.Store).Where(b => b.AppUserId == user.Id && b.RestuorantId == restuorant.Id).ToList();
            Order order = new Order
            {
                AppUserId = user.Id,
                RestuorantId = restuorant.Id,
                IsDelivery = IsDelivery,
                IsCard = Iscard,
                CardMonth = "1",
                CardYear = "1",
                CardNumber = 1,
                Owner = "ahad",
                Cvv = 78,
                Date = DateTime.UtcNow.AddHours(3),
            };

            if (Adress != null)
            {
                order.Adress = Adress;
            }
            foreach (var item in basketitems)
            {
                order.TotalPrice += item.Restuorant.CampaignId == null ? item.Count * item.Product.Price :
                     (item.Restuorant.CampaignId == null ? item.Count * item.Product.Price : item.Count * (item.Product.Price * (100 - item.Restuorant.Campaign.CampaignPercent) / 100));

                OrderItems orderItems = new OrderItems
                {
                    Order = order,
                    Product = item.Product,
                    AppUserId = user.Id,
                    RestuorantId = restuorant.Id,
                    Count=item.Count
                    
                    
                };
                
                _context.OrderItems.Add(orderItems);
            }
            if (IsDelivery)
            {
                order.TotalPrice = order.TotalPrice + 2;
            }
            _context.Orders.Add(order);
            _context.BasketItems.RemoveRange(basketitems);
            _context.SaveChanges();
            Order order2 = order;
            if (ResUser.ConnectionId!=null)
            {
                await Clients.Client(ResUser.ConnectionId).SendAsync("thereisorder", new
                {
                    restuorantid = restuorantid,
                    userid = user.Id,
                    orderid = order2.Id
                });
            }


        }
        public async Task OrderProductStore(string storeid, string Isdelivery, string Adress, string IsCard)
        {
            bool IsDelivery = bool.Parse(Isdelivery);
            bool Iscard = bool.Parse(IsCard);
            int StoreId = int.Parse(storeid);

            Store store = _context.Stores.FirstOrDefault(r => r.Id == StoreId);
            AppUser user = await _userManager.FindByNameAsync(Context.User.Identity.Name);
            AppUser Storeuser = await _userManager.FindByIdAsync(store.AppUserId);
            List<BasketItem> basketitems = _context.BasketItems.Include(bi => bi.Product).Include(bi => bi.AppUser).Include(bi => bi.Restuorant).Include(bi => bi.Store).Where(b => b.AppUserId == user.Id && b.StoreId == store.Id).ToList();
            Order order = new Order
            {
                AppUserId = user.Id,
                StoreId = store.Id,
                IsDelivery = IsDelivery,
                IsCard = Iscard,
                CardMonth = "1",
                CardYear = "1",
                CardNumber = 1,
                Owner = "ahad",
                Cvv = 78,
                Date = DateTime.UtcNow.AddHours(3),
            };

            if (Adress != null)
            {
                order.Adress = Adress;
            }
            foreach (var item in basketitems)
            {
                order.TotalPrice += item.Count * item.Product.Price;
                OrderItems orderItems = new OrderItems
                {
                    Order = order,
                    Product = item.Product,
                    AppUserId = user.Id,
                    StoreId = store.Id,
                    Count = item.Count


                };

                _context.OrderItems.Add(orderItems);
            }
            if (IsDelivery)
            {
                order.TotalPrice = order.TotalPrice + 2;
            }
            _context.Orders.Add(order);
            _context.BasketItems.RemoveRange(basketitems);
            _context.SaveChanges();
            Order order2 = order;
            await Clients.Client(Storeuser.ConnectionId).SendAsync("thereisorder", new
            {
                restuorantid = storeid,
                userid = user.Id,
                orderid = order2.Id
            });


        }
        public async Task OrderAccept(string orderid)
        {
            int orderId = int.Parse(orderid);
            Order order = _context.Orders.FirstOrDefault(o => o.Id==orderId);
            AppUser user = await _userManager.FindByIdAsync(order.AppUserId);
            order.IsAccept = true;
            _context.SaveChanges();
            await Clients.Client(user.ConnectionId).SendAsync("orderaccept");
        }
        public async Task OrderReject(string orderid)
        {
            int orderId = int.Parse(orderid);
            Order order = _context.Orders.FirstOrDefault(o => o.Id == orderId);
            AppUser User = await _userManager.FindByIdAsync(order.AppUserId);
            _context.Orders.Remove(order);
            _context.SaveChanges();
            if (order.RestuorantId!=null)
            {

            Restuorant restuorant = _context.Restuorants.FirstOrDefault(r=>r.Id==order.RestuorantId);
            await Clients.Client(User.ConnectionId).SendAsync("rejected",restuorant.Id);
            }
            else
            {
                Store store = _context.Stores.FirstOrDefault(r => r.Id == order.StoreId);
                await Clients.Client(User.ConnectionId).SendAsync("rejected", store.Id);
            }
        }
        public async Task OrderReady(string orderid)
        {
            int orderId = int.Parse(orderid);
            Order order = _context.Orders.FirstOrDefault(o => o.Id == orderId);
          
            AppUser User = await _userManager.FindByIdAsync(order.AppUserId);
            order.OrderComleete = true;
            _context.SaveChanges();
            await Clients.Client(User.ConnectionId).SendAsync("orderready",new { 
            id=order.Id
            });
            if (order.IsDelivery)
            {
            await  FindCourier(orderid);
            }
        }
        public async Task FindCourier(string orderid)
        {
            int orderId = int.Parse(orderid);
            Order order = _context.Orders.FirstOrDefault(o => o.Id == orderId);
            AppUser User = await _userManager.FindByIdAsync(order.AppUserId);
           
            List<AppUser> Couriers = _userManager.Users.Where(c => c.Role == "Courier"&&c.ConnectionId!=null).ToList();
            foreach (var item in Couriers)
            {
                    await Clients.Client(item.ConnectionId).SendAsync("isacceptdelivery", new
                    {
                        id=order.Id
                    });
            }
        }
        public async Task CourierWasAppointed(string orderid)
        {
            int OrderId = int.Parse(orderid);
            Order order = _context.Orders.Include(o=>o.AppUser).FirstOrDefault(o => o.Id == OrderId&&o.IsCourierFind==false);
            AppUser Courier = await _userManager.FindByNameAsync(Context.User.Identity.Name);
            if (order==null)
            {
                await Clients.Client(Courier.ConnectionId).SendAsync("exsist",new
                {
                    id = order.Id
                });
            }
            else
            {
            order.CourierID = Courier.Id;
            order.IsCourierFind = true;
            _context.SaveChanges();
            AppUser appUser = await _userManager.FindByIdAsync(order.AppUserId);
            await Clients.Client(appUser.ConnectionId).SendAsync("courierwasappointed",new
            {
                id = order.Id
            });
            }
        }
        public async Task Order(string orderid)
        {
            int orderId = int.Parse(orderid);
            Order order = _context.Orders.FirstOrDefault(o => o.Id == orderId);
            AppUser User = await _userManager.FindByIdAsync(order.AppUserId);
            if (!order.IsDelivery)
            {
                order.IsOrderComlete = true;
            }
            if (order.IsDelivery)
            {
            await Clients.Client(User.ConnectionId).SendAsync("order", new
            {
                id = order.Id
            });
            }
            order.IsCourierTaked = true;

            _context.SaveChanges();

        }
        public async Task OrderCompleted(string orderid)
        {
            int OrderId = int.Parse(orderid);
            Order order = _context.Orders.Include(o => o.AppUser).FirstOrDefault(o => o.Id == OrderId);
            order.IsOrderComlete = true;
            AppUser appUser = await _userManager.FindByIdAsync(order.AppUserId);
            if (order != null)
            {
                await Clients.Client(appUser.ConnectionId).SendAsync("completed", new
                {
                    id = order.Id
                });
            }
            order.IsOrderComlete = true;
            _context.SaveChanges();
        }





        public override Task OnConnectedAsync()
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                AppUser user = _userManager.FindByNameAsync(Context.User.Identity.Name).Result;

                user.ConnectionId = Context.ConnectionId;

                var result = _userManager.UpdateAsync(user).Result;

                Clients.All.SendAsync("UserConnected", user.Id);
            }

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                AppUser user = _userManager.FindByNameAsync(Context.User.Identity.Name).Result;

                user.ConnectionId = null;

                var result = _userManager.UpdateAsync(user).Result;

                Clients.All.SendAsync("UserDisConnected", user.Id);
            }

            return base.OnDisconnectedAsync(exception);
        }

    }
}
