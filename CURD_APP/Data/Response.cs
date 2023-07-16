namespace CURD_APP.Data
{
    public class Response<T>
    {
        public string Message { get; set; }
        public T Data { get; set; }

        public Response(string message, T data)
        {

            Message = message;
            Data = data;
        }
    }
}
