namespace Entities.Exceptions
{
    //filtrelemede yapılacak hatayı yönetebileceğimiz bir istisna tanımı gerçekleştirdik
    public class PriceOutofRangeBadRequestException:BadRequestException 
    {
        public PriceOutofRangeBadRequestException() : base ("maximum price should be less than 1000 an greater than 10. ")
        {
            
        }
    }
}
