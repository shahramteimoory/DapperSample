using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Reflection;
using System;

namespace DapperSample.MyCommand
{
    public interface ICommandText
    {
        string GetProducts {  get; }
        string GetProductById {  get; }
        string AddProduct {  get; }
        string UpdateProduct { get; }

        string GetProductName { get; }
        string RemoveProduct { get; }
    }
    public class CommandText : ICommandText
    {
        public string GetProducts => "Select * from Products";

        public string GetProductById => "Select * from Products where Id=@Id";

        public string AddProduct => "Inser Into Products (ProductName,Size,Nooeh,Price,Status,Insert_DateTime,Insert_ByUserID,IsDeleted,Delete_DateTime,Delete_ByUserID,Update_DateTime,Update_ByUserID)values" +
            "(@ProductName,@Size,@Nooeh,@Price,@Status,@Insert_DateTime,@Insert_ByUserID,@IsDeleted,@Delete_DateTime,@Delete_ByUserID,@Update_DateTime,@Update_ByUserID)";

        public string UpdateProduct => "Update Products Set ProductName=@ProductName,Size=@Size,Nooeh=@Nooeh,Price=@Price,Status=@Status,Insert_DateTime=@Insert_DateTime,Insert_ByUserID=@Insert_ByUserID,IsDeleted=@IsDeleted,Delete_DateTime=@Delete_DateTime,Delete_ByUserID=@Delete_ByUserID,Update_DateTime=@Update_DateTime,Update_ByUserID=@Update_ByUserID";

        public string GetProductName => "Select * from Products where ProductName=@ProductName";

        public string RemoveProduct => "Delete from Products where Id=@Id";

//        select * from [myContact]   خوندن همه از my contact
//        select FirstName, LastName from[myContact] خوندن دو ستون از mycontact
//select* from[mycontact] where name = n'ایمان'          s سزچ کردن یک اسم از تیبل یا گذاشتن یک شرط
//select* from[mycontact] where name = 'ایمان' and age = 30                      گذاشتن 22 شزط
//select* from[mycontact] where name = 'ایمان' or age = 30                   جای دوشزط گذاشتن یا
//Insert into[mycontact] (FirstName, LastName, Age, Mobile, Email) values(n'سارا', n'سارایی', inalakiye 12 , '0935*****' ,'sara@yahoo.com)                 دستور اینسرت کردن به دیتا بیس 
//update[mycontact] set Age = 12 where contactID = 3        دستور آپدیت دیتا بیس
//Delete from [mycontact] where contactID = 3                         دستور دلیت در دیتا بیس
//select * from mycontect
//"select * from contact where FirstName like @Parameter or LastName like @Parameter "; سرچ
    }
}
