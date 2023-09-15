namespace Entities.Exceptions
{
    //sealed zırhlama yani hiç bir şekilde kalıtılmayacak 
    /*Bu kod parçası, Entities.Exceptions ad alanı altında BookNotFoundException adında bir özel istisna (exception) sınıfı tanımlar.
      Bu sınıf, belirli bir kitabın bulunamaması durumunda fırlatılan istisnaları temsil etmek için kullanılır. */
    /*BookNotFoundException, NotFoundException sınıfından kalıtım alır,
     yani NotFoundException sınıfının özelliklerini ve davranışlarını devralır.  */
    public sealed class BookNotFoundException : NotFoundException
        {
            public BookNotFoundException(int id) : base($"The Book With id: {id} could not found.")
            {

            }
        
        }
}
