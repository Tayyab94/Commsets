using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLibrary;
using BusinessLibrary.Classified;

namespace finalProject.Models
{
    public static class ModelHelper
    {

        //Generic Method for DropDownList
        public static List<SelectListItem> ToSelectList(this IEnumerable<IListable> values)
        {
            List<SelectListItem> tempList = new List<SelectListItem>();
            foreach (var item in values)
            {
                tempList.Add(new SelectListItem { Text = item.Name, Value = Convert.ToString(item.ID) });
            }
            tempList.TrimExcess();
            return tempList;
        }


        public static List<AdvSumModel>ToAdvertiseSumModel(this List<Advertisement> advertisement)
        {
            List<AdvSumModel> tempList = new List<AdvSumModel>();
            foreach (var adv in advertisement)
            {
                AdvSumModel m = new AdvSumModel();
                m.Id = adv.ID;
                m.Title = adv.Title;
                m.Price = adv.price;
                m.postedDate = adv.PostedOn;
                m.Cat = adv.SubCatagory.Catagory.Name;
                m.Status = adv.Status.Name;
              
                m.ImgUrl = (adv.Images.Count > 0) ? adv.Images.First().Url : "/Images/temp/nophoto.png";
                tempList.Add(m);
            }
            tempList.TrimExcess();
            return tempList;
           
        }

        //public static List<AdvSumModel> ToDetailAdvertiseSumModel(this List<Advertisement> advertisement)
        //{
        //    List<AdvSumModel> tempList = new List<AdvSumModel>();
        //    foreach (var adv in advertisement)
        //    { AdvSumModel m = new AdvSumModel();
        //        m.Id = adv.ID;
        //        m.Title = adv.Title;
        //        m.Price = adv.price;
        //        m.postedDate = adv.PostedOn;
        //        m.ImgUrl = (adv.Images.Count > 0) ? adv.Images.First().Url : "/Images/temp/nophoto.png";
        //        tempList.Add(m);
        //    }
        //    tempList.TrimExcess();
        //    return tempList;
        //}

        public static AdvSumModel DetailAdv(Advertisement a)
        {
            AdvSumModel tem = new AdvSumModel();
            tem.Id = a.ID;
            tem.ImgUrl= (a.Images.Count > 0) ? a.Images.First().Url : "/Images/temp/nophoto.png";
            tem.Price = a.price;


            return tem;
        }


        public static List<CatagoryModel> ToSelectCataList(this List<Catagory> val)
        {
            List<CatagoryModel> temp = new List<CatagoryModel>();

            foreach (var item in val)
            {
                CatagoryModel c = new CatagoryModel();
                c.Id = item.ID;
                c.Name = item.Name;
                temp.Add(c);
            }
            temp.TrimExcess();
            return temp; 
        }
    }
}