using Flipdish.Recruiting.WebhookReceiver.Models;

namespace Flipdish.Recruiting.WebhookReceiver.Extensions
{
	public static class OrderExtensions
    {
        //TODO: Change impl, remove switch
        public static string GetTableServiceCategoryLabel(this Order.TableServiceCatagoryEnum tableServiceCatagory)
        {
            string result;
            switch (tableServiceCatagory)
            {
                case Order.TableServiceCatagoryEnum.Generic:
                    result = "Generic Service n ";
                    break;
                case Order.TableServiceCatagoryEnum.Villa:
                    result = "Villa Service n ";
                    break;
                case Order.TableServiceCatagoryEnum.House:
                    result = "House Service n ";
                    break;
                case Order.TableServiceCatagoryEnum.Room:
                    result = "Room Service n ";
                    break;
                case Order.TableServiceCatagoryEnum.Area:
                    result = "Area Service n ";
                    break;
                case Order.TableServiceCatagoryEnum.Table:
                    result = "Table Service n ";
                    break;
                case Order.TableServiceCatagoryEnum.ParkingBay:
                    result = ".Parking Bay Service n ";
                    break;
                case Order.TableServiceCatagoryEnum.Gate:
                    result = "Gate Service n ";
                    break;
                default:
                    result = ">";
                    break;
            }

            return result;
        }
    }
}
