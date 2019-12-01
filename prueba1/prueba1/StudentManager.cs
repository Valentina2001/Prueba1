using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace prueba1
{
    public class StudentManager
    {
        private IDbConnection CreateConnection()
        {
            SqlConnection conn = new SqlConnection(@"Server=.\SQLEXPRESS;Database=prueba1;Trusted_Connection=True;");
            conn.Open();
            return conn;
        }

        private void CreateParameter<T>(IDbCommand cmd, string name, T value)
        {
            IDbDataParameter prm = cmd.CreateParameter();
            prm.ParameterName = name;
            prm.Value = value;
            cmd.Parameters.Add(prm);
        }

        public List<Student> GetStudents()
        {
            List<Student> students = new List<Student>();
            using (IDbConnection conn = CreateConnection())
            {
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT ID, name, lastName, address, phone FROM Student";
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            students.Add(new Student()
                            {
                                ID = Convert.ToInt32(dr["ID"]),
                                Name = dr["name"].ToString(),
                                LastName = dr["lastName"].ToString(),
                                Address = dr["address"].ToString(),
                                Phone = dr["phone"].ToString(),
                            });
                        }
                    }
                }
            }
            return students;
        }

        public void Write(Student student)
        {
            using(IDbConnection conn = CreateConnection())
            {
                using (IDbTransaction trx = conn.BeginTransaction())
                {
                    
                    try
                    {
                        using (IDbCommand cmd = conn.CreateCommand())
                        {

                            cmd.Transaction = trx;

                            cmd.CommandText = "INSERT INTO student(name, lastName, address, phone) VALUES(@Name, @LastName, @Address, @Phone)";

                            CreateParameter(cmd, "Name", student.Name);
                            CreateParameter(cmd, "LastName", student.LastName);
                            CreateParameter(cmd, "Address", student.Address);
                            CreateParameter(cmd, "Phone", student.Phone);

                            cmd.ExecuteNonQuery();

                            cmd.CommandText = "INSERT INTO logs(action, createDate) VALUES(@action, @createDate)";
                            CreateParameter(cmd, "action", "New student created");
                            CreateParameter(cmd, "createDate", DateTime.Now);
                            cmd.ExecuteNonQuery();
                        }
                        trx.Commit();

                    }
                    catch
                    {
                        trx.Rollback();
                    }

                }
                
            }
        }

        public void Update(Student student)
        {
            using(IDbConnection conn = CreateConnection())
            {
                using(IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "UPDATE Student SET Name=@Name, LastName=@LastName, Address=@Address, Phone=@Phone WHERE ID=@ID";

                    CreateParameter(cmd, "ID", student.ID);
                    CreateParameter(cmd, "Name", student.Name);
                    CreateParameter(cmd, "LastName", student.LastName);
                    CreateParameter(cmd, "Address", student.Address);
                    CreateParameter(cmd, "Phone", student.Phone);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int ID)
        {
            using(IDbConnection conn = CreateConnection())
            {
                using(IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Student WHERE ID=@ID";
                    CreateParameter(cmd, "ID", ID);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
