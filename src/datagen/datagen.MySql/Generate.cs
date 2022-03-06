namespace datagen.MySql
{
    public class Generate : IGenerate
    {
        private readonly string _connectionString;

        public Generate(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddRow()
        {
            throw new NotImplementedException();
        }

        public void FillColumn()
        {
            throw new NotImplementedException();
        }

        public void FillDB()
        {
            throw new NotImplementedException();
        }

        public void FillTable()
        {
            throw new NotImplementedException();
        }
    }
}