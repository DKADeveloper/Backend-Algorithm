namespace fguprsvo.Alogs
{
    //Интерфейс для реализации классов
    public interface IAlgo
    {
        public IFormFile File { get; set; }

        public Task<string> RunAsync();
    }
}
