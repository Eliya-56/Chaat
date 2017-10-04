using SimpleChat.Requests;
using SimpleChat.UserData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SimpleChat
{

    delegate void RequestHandler(ChaatStream stream, object data);

    public class RequestInfo
    {
        public ChaatRequest Request { get; }
        public object Data { get; }

        public RequestInfo(ChaatRequest request, object data)
        {
            Request = request;
            Data = data;
        }
    }


    public class ChaatStream
    {
        private BinaryReader reader;
        private BinaryWriter writer;
        public IPEndPoint EndPoint { get; private set; }

        private Dictionary<ChaatRequest, Type> requests = new Dictionary<ChaatRequest, Type>();

        public ChaatStream(Stream stream)
        {
            reader = new BinaryReader(stream);
            writer = new BinaryWriter(stream);
            RegisterRequest<TextMessage>(ChaatRequest.Message);
            RegisterRequest<SignUp>(ChaatRequest.SignUp);
            RegisterRequest<SignIn>(ChaatRequest.SignIn);
            RegisterRequest<SigningResponse>(ChaatRequest.SigningResponse);
            RegisterRequest<GiveMeActiveUsers>(ChaatRequest.GiveMeActiveUsers);
            RegisterRequest<SendActiveUsers>(ChaatRequest.SendActiveUsers);
        }

        public ChaatStream(Stream stream, IPEndPoint EndPoint) : this(stream)
        {
            this.EndPoint = EndPoint;
        }

        private void RegisterRequest<T>(ChaatRequest request)
        {
            requests[request] = typeof(T);
        }

        public RequestInfo Read()
        {
            var r = (ChaatRequest)reader.ReadByte();
            Type t = requests[r];
            object data = Activator.CreateInstance(t);
            foreach (var prop in t.GetProperties())
            {
                if (prop.PropertyType == typeof(string))
                    prop.SetValue(data, reader.ReadString());
                else if (prop.PropertyType == typeof(int))
                    prop.SetValue(data, reader.ReadInt32());
                else if (prop.PropertyType == typeof(bool))
                    prop.SetValue(data, reader.ReadBoolean());
                else if (prop.PropertyType == typeof(AuthData))
                {
                    var login = reader.ReadString();
                    var password = reader.ReadString();
                    prop.SetValue(data, new AuthData(login, password));
                }
                else if (prop.PropertyType == typeof(SignType))
                {
                    prop.SetValue(data, (SignType)reader.ReadInt32());
                }
                else if(prop.PropertyType == typeof(DateTime))
                {
                    var time = reader.ReadInt64();
                    prop.SetValue(data, DateTime.FromBinary(time));
                }
                else if(prop.PropertyType == typeof(string[]))
                {
                    var size = reader.ReadInt32();
                    List<string> strs = new List<string>();
                    for (int i = 0; i < size; i++)
                    {
                        strs.Add(reader.ReadString());
                    }
                    prop.SetValue(data, strs.ToArray());
                }
            }
            return new RequestInfo(r, data);
        }

        public void Write<T>(T data)
        {
            Type objType = typeof(T);
            ChaatRequest request = requests.First(f => f.Value == objType).Key;
            writer.Write((byte)request);

            foreach (var prop in objType.GetProperties())
            {
                if (prop.PropertyType == typeof(string))
                    writer.Write((string)prop.GetValue(data));
                else if (prop.PropertyType == typeof(int))
                    writer.Write((int)prop.GetValue(data));
                else if (prop.PropertyType == typeof(bool))
                    writer.Write((bool)prop.GetValue(data));
                else if (prop.PropertyType == typeof(AuthData))
                {
                    var user = (AuthData)prop.GetValue(data);
                    writer.Write(user.Login);
                    writer.Write(user.Password);
                }
                else if (prop.PropertyType == typeof(SignType))
                {
                    writer.Write((int)prop.GetValue(data));
                }
                else if(prop.PropertyType == typeof(DateTime))
                {
                    long time = ((DateTime)prop.GetValue(data)).ToBinary();
                    writer.Write(time);
                }
                else if (prop.PropertyType == typeof(string[]))
                {
                    var strs = (string[])prop.GetValue(data);
                    var size = strs.Length;
                    writer.Write(size);
                    for (int i = 0; i < size; i++)
                    {
                        writer.Write(strs[i]);
                    }
                }
            }
        }
    }
}
