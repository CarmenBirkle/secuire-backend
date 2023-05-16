namespace PWManagerService.Factory
{
    public interface IDataHandler<T>
    {
        public string InsertData(T data);
        public void DeleteData(int Id);
        public void UpdateData(T data);
        public List<T> GetData();
    }
}
