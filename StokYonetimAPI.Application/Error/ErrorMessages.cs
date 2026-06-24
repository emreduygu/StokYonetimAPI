using System;
using System.Collections.Generic;
using System.Text;

namespace StokYonetimAPI.Application.Error
{
    public static class ErrorMessages
    {
        public const string ProductNameEmpty = "Ürün adı boş olamaz";
        public const string StockNegative = "Stok sıfırdan az olamaz";
        public const string PriceNegative = "Fiyat sıfırdan az olamaz";
        public const string ProductNotFound = "Ürün bulunamadı";
        public const string IDNegative = "ID negatif olamaz";
        public const string UsernameExist = "Bu kullanıcı adı zaten alınmış";
        public const string UsernamePasswordError = "Kullanıcı adı veya şifre hatalı";
        public const string UserNotFound = "Kullanıcı bulunamadı";
        public const string UsernameEmpty = "Kullanıcı adı boş olamaz";
        public const string PasswordEmpty = "Şifre boş olamaz";


    }
}
