namespace Entities.DataTransferObject
{
    //bu formatta yazmamızıın sebebi xml çıktısını alırkenn daha geçerli ve okunaklı şekilde alabilmek için
    public record BookDto
    {
        public int Id { get; init; }

        public String Title { get; init; }

        public decimal Price { get; init; }
    };
}
