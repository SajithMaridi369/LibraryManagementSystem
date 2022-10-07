using System;
using System.Collections;
using System.Data.SqlClient;
namespace Sample
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.Clear();
            string con_str = @"Server=localhost\SQLEXPRESS;Database=LMS;Trusted_Connection=True;";
            SqlConnection con = new SqlConnection(con_str);
            con.Open();
            while (true)
            {
                Console.WriteLine("\nLogin as : \n1.Admin\n2.Student\n3.Exit");
                int userChoice = Convert.ToInt32(Console.ReadLine());
                Admin admin_obj = new Admin();
                Student student_obj = new Student();
                if (userChoice == 1)
                    admin_obj.adminLogin(con);
                else if (userChoice == 2)
                {
                    Console.WriteLine("\nEnter your choice : \n1.Login\n2.Register");
                    int student_choice = Convert.ToInt32(Console.ReadLine());
                    if (student_choice == 1)
                        student_obj.studentLogin(con);
                    else if (student_choice == 2)
                        student_obj.studentRegister(con);
                    else
                    {
                        Console.WriteLine("Entered invalid choice ...!!");
                        continue;
                    }
                }
                else
                    break;
            }
            con.Close();
        }
    }
    class Admin
    {
        // public static ArrayList Book_list = new ArrayList();
        public void adminLogin(SqlConnection con)
        {
            Console.WriteLine("Enter Admin id : ");
            string admin_id = Console.ReadLine();
            if (admin_id == "Admin")
            {
                Console.WriteLine("Enter Password : ");
                string password = Console.ReadLine();
                if (password == "1234")
                {
                    displayAdminOperations(con);
                }
                else
                {
                    Console.WriteLine("Invalid password...!!");
                    return;
                }
            }
            else
            {
                Console.WriteLine("Invalid Admin ID...!!");
            }
        }
        public static void displayAdminOperations(SqlConnection con)
        {
            Console.Clear();
            while (true)
            {
                Console.WriteLine("\nEnter operation to be performed :\n1.Display Users\n2.Add Books\n3.Book Search\n4.Book Update\n5.View Orders\n6.Display Available Books\n7.Logout");
                int admin_choice = Convert.ToInt32(Console.ReadLine());
                switch (admin_choice)
                {
                    case 1:
                        displayAvailableUsers();
                        break;
                    case 2:
                        addBook(con);
                        break;
                    case 3:
                        searchBook(con);
                        break;
                    case 4:
                        updateBook(con);
                        break;
                    case 5:
                        viewOrders();
                        break;
                    case 6:
                        viewBooks(con);
                        break;
                    case 7: return;
                    default:
                        Console.WriteLine("Entered Invalid choice...!!!");
                        break;
                }
            }
        }
        public static void displayAvailableUsers()
        {
            if (Student.students_list.Count == 0)
            {
                Console.WriteLine("\nNo Users registered...!!");
                return;
            }
            Console.WriteLine("\nAvailable users are : ");
            foreach (StudentDatabase obj in Student.students_list)
            {
                Console.WriteLine(obj.id + "\t" + obj.name + "\tBook Taken : " + obj.book_taken);
            }
        }
        public static void addBook(SqlConnection con)
        {
            Console.WriteLine("Enter Book name : ");
            string book_name = Console.ReadLine();
            string query = "select * from Book";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                if (dr.GetValue(1).ToString() == book_name)
                {
                    Console.WriteLine("Book already exists in library...!!");
                    return;
                }
            }
            dr.Close();
            Console.WriteLine("Enter Author name : ");
            string book_author = Console.ReadLine();
            Console.WriteLine("Enter Genre : ");
            string genre = Console.ReadLine();
            Console.WriteLine("Enter Number of Copies Shipped : ");
            int no_of_copies = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter cost of the book : ");
            int cost = Convert.ToInt32(Console.ReadLine());
            string book_id = "B-" + Convert.ToString((Book_list.Count) + 1);
            query = "insert into Book('" + book_id + "', '" + book_name + "', '" + book_author + "', '" + genre + "', " + no_of_copies + ", " + cost + ")";
            cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
            Console.WriteLine("Book added to Library.");
        }
        public static void searchBook(SqlConnection con)
        {
            Console.WriteLine("Enter Book name : ");
            string book_name = Console.ReadLine();
            string query = "select * from Book";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            int bk_count = 0;
            while (dr.Read())
            {
                if (dr.GetValue(1).ToString() == book_name)
                {
                    Console.WriteLine(dr.GetValue(0).ToString() + "\t" + dr.GetValue(1).ToString() + "\t" + dr.GetValue(2).ToString() + "\t" + dr.GetValue(3).ToString() + "\t" + dr.GetValue(4).ToString() + "\t" + "$" + dr.GetValue(5).ToString());
                    return;
                }
                bk_count += 1;
            }
            dr.Close();
            if (bk_count == 0)
            {
                Console.WriteLine("Library is Empty...!!!");
                return;
            }
            else
            {
                Console.WriteLine("Book not found...!!!");
            }
        }
        public static void updateBook(SqlConnection con)
        {
            string query = "select count(*) from Book";
            SqlCommand cmd = new SqlCommand(query, con);
            int res = (int)cmd.ExecuteScalar();
            if (res == 0)
            {
                Console.WriteLine("Library is Empty...!!!");
                return;
            }
            Console.WriteLine("Enter Book ID to perform update(Enter Serial Number) : ");
            int serial_number = Convert.ToInt32(Console.ReadLine());
            if (serial_number > res)
            {
                Console.WriteLine("Invalid Book ID...!!!");
                return;
            }
            string book_id = "B-" + Convert.ToString((Book_list.Count) + 1);
            query = "select * from Book where book_id=" + book_id + ";";
            cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            dr.Read();
            Console.WriteLine(dr.GetValue(0).ToString() + "\t" + dr.GetValue(1).ToString() + "\t" + dr.GetValue(2).ToString() + "\t" + dr.GetValue(3).ToString() + "\t" + dr.GetValue(4).ToString() + "\t$" + dr.GetValue(5).ToString());
            dr.Close();

            Console.WriteLine("Update Details : ");
            Console.WriteLine("Enter book name : ");
            string bk_nm = Console.ReadLine();
            Console.WriteLine("Enter author name : ");
            string ath_nm = Console.ReadLine();
            Console.WriteLine("Enter Genre : ");
            string genre = Console.ReadLine();
            Console.WriteLine("Enter Number of Copies Shipped : ");
            int no_of_copies = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter cost of the book : ");
            int cost = Convert.ToInt32(Console.ReadLine());
            query = "update Book set book_name = " + bk_nm + ", book_author = " + ath_nm + ", genre = " + genre + ", no_of_copies = " + no_of_copies + ", cost = " + cost + ";";
            cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
        }
        public static void viewOrders()
        {
            if (Student.students_list.Count == 0)
            {
                Console.WriteLine("No users registered..!!");
                return;
            }
            int order_count = 0;
            foreach (StudentDatabase obj in Student.students_list)
            {
                if (!obj.book_taken.Equals("None"))
                {
                    Console.WriteLine(obj.id + "\t" + obj.name + "\t" + obj.book_taken + "\t" + obj.return_time + "-days to return");
                    order_count += 1;
                }
            }
            if (order_count == 0)
                Console.WriteLine("No orderes received...!!");
        }
        public static void viewBooks(SqlConnection con)
        {
            string query = "select count(*) from Book;";
            SqlCommand cmd = new SqlCommand(query, con);
            int res = (int)cmd.ExecuteScalar();
            if (res == 0)
            {
                Console.WriteLine("Library is empty...!!");
                return;
            }
            query = "select * from Book;";
            cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            Console.WriteLine();
            while (dr.Read())
            {
                Console.WriteLine(dr.GetValue(0).ToString() + "\t" + dr.GetValue(1).ToString() + "\t" + dr.GetValue(2).ToString() + "\t" + dr.GetValue(3).ToString() + "\t" + dr.GetValue(4).ToString() + "\t" + "$" + dr.GetValue(5).ToString());
            }
            dr.Close();
        }
    }
    class Student
    {
        // public static ArrayList students_list = new ArrayList();
        // public static StudentDatabase logged_in_user;
        public void studentRegister(SqlConnection con)
        {
            Console.WriteLine("Enter your user name : ");
            string student_name = Console.ReadLine();
            string query = "select count(*) from StudentTable where sname = " + student_name + ";";
            SqlCommand cmd = new SqlCommand(query, con);
            int res = (int)cmd.ExecuteScalar();
            if (res != 0)
            {
                Console.WriteLine("Student already Registered...!!");
                return;
            }
            query = "select count(*) from StudentTable;";
            cmd = new SqlCommand(query, con);
            res = (int)cmd.ExecuteScalar();
            Console.WriteLine("Enter your password(Needs to be atleast 4 characters) : ");
            string password = Console.ReadLine();
            String student_id = "S-" + Convert.ToString(res + 1);
            query = "insert into StudentTable vlues(" + student_id + ", " + student_name + ", " + password + ", 'None', -1);";
            cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
            Console.WriteLine("Student sucessfully registered.");
        }
        public void studentLogin(SqlConnection con)
        {
            Console.WriteLine("Enter your user name : ");
            string user_name = Console.ReadLine();
            string query = "select count(*) from StudentTable where sname = " + user_name + ";";
            SqlCommand cmd = new SqlCommand(query, con);
            int res = (int)cmd.ExecuteScalar();
            if (res == 0)
            {
                Console.WriteLine("User not Regsitered...!!");
                return;
            }
            Console.WriteLine("Enter your password(Needs to be atleast 4 characters) : ");
            string password = Console.ReadLine();
            query = "select * from StudentTable where sname = " + user_name + ";";
            cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            dr.Read();
            if (dr.GetValue(2).ToString().Equals(password))
            {
                displayUserOperations(con, user_name);
                return;
            }
            else
            {
                Console.WriteLine("Entered invalid password...!!");
                return;
            }
        }
        public static void displayUserOperations(SqlConnection con, string user_name)
        {
            Console.Clear();
            while (true)
            {
                Console.WriteLine("\nEnter your choice : \n1.Book Search\n2.View return Date info\n3.Return Book\n4.Place Order\n5.Logout");
                int user_choice = Convert.ToInt32(Console.ReadLine());
                switch (user_choice)
                {
                    case 1:
                        searchBook(con);
                        break;
                    case 2:
                        returnDateDisplay(con, user_name);
                        break;
                    case 3:
                        returnBook(con, user_name);
                        break;
                    case 4:
                        placeOrder(con, user_name);
                        break;
                    case 5:
                        return;
                    default:
                        Console.WriteLine("Entered invalid choice...!!!");
                        break;
                }
            }
        }
        public static void searchBook(SqlConnection con)
        {
            Console.WriteLine("Enter Book name : ");
            string book_name = Console.ReadLine();
            string query = "select * from Book";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            int bk_count = 0;
            while (dr.Read())
            {
                if (dr.GetValue(1).ToString() == book_name)
                {
                    Console.WriteLine(dr.GetValue(0).ToString() + "\t" + dr.GetValue(1).ToString() + "\t" + dr.GetValue(2).ToString() + "\t" + dr.GetValue(3).ToString() + "\t" + dr.GetValue(4).ToString() + "\t" + "$" + dr.GetValue(5).ToString());
                    return;
                }
                bk_count += 1;
            }
            dr.Close();
            if (bk_count == 0)
            {
                Console.WriteLine("Library is Empty...!!!");
                return;
            }
            else
            {
                Console.WriteLine("Book not found...!!!");
            }
        }
        public static void returnDateDisplay(SqlConnection con, string user_name)
        {
            string query = "select * from StudentTable where sname = " + user_name + ";";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            dr.Read();
            if (dr.GetValue(3).ToString().Equals("None"))
            {
                Console.WriteLine("No book taken...!!");
                return;
            }
            Console.WriteLine("\nBook taken : " + dr.GetValue(3).ToString() + "\nReturn before : " + dr.GetValue(4).ToString() + " days.");
        }
        public static void returnBook(SqlConnection con, string user_name)
        {
            string query = "select * from StudentTable where sname = " + user_name + ";";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            dr.Read();
            if (dr.GetValue(3).ToString().Equals("None"))
            {
                Console.WriteLine("No book taken...!!");
                return;
            }
            query = "select * from StudentTable where sname = user_name";
            cmd = new SqlCommand(query, con);
            dr = cmd.ExecuteReader();
            dr.Read();
            string bk_taken = dr.GetValue(3).ToString();
            query = "select * from Book where book_id = " + bk_taken + ";";
            cmd = new SqlCommand(query, con);
            dr = cmd.ExecuteReader();
            dr.Read();
            int copies = dr.GetValue(4);
            query = "update Book set no_of_copies = " + (copies + 1) + " where book_id = " + bk_taken + ";";
            cmd = new SqlCommand();
            cmd.ExecuteNonQuery();
            Console.WriteLine("Successfully returned...");
        }
        public static void placeOrder(SqlConnection con, string user_name)
        {
            string query = "select count(*) from Book;";
            SqlCommand cmd = new SqlCommand(query, con);
            int cnt = cmd.ExecuteScalar();
            if (cnt == 0)
            {
                Console.WriteLine("Library is empty...!!");
                return;
            }
            query = "select * from StudentTable where sname = " + user_name + "";
            cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            dr.Read();
            string puchased = dr.GetValue(3).ToString();
            if (puchased.Equals("None"))
            {
                Console.WriteLine("Return taken book to place a new order...!!");
                return;
            }
            Console.WriteLine("Enter Book ID to place order : ");
            int entered_id = Convert.ToInt32(Console.ReadLine());
            if (entered_id > cnt)
            {
                Console.WriteLine("Entered Invalid ID...!!");
                return;
            }
            string bk_id = "B-" + Convert.ToString(entered_id);
            query = "select * from Book where id = " + bk_id + ";";
            cmd = new SqlCommand(query, con);
            dr = cmd.ExecuteReader();
            dr.Read();
            string bk_name = dr.GetValue(1).ToString();
            int copies = dr.GetValue(4);
            if (copies == 0)
            {
                Console.WriteLine("No copies are available...!!");
                return;
            }
            query = "update StudentTable set booktaken = " + bk_name + ", returntime = 10";
            cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
            query = "update Book set no_of_copies = " + (copies - 1) + " where book_id = " + entered_id + ";";
            cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
            Console.WriteLine("Order Placed");
        }
    }
}
/*
    Book :      book_id(varchar(69), not null)
                book_name(varchar(69), not null)
                book_author(varchar(69), not null)
                genre(varchar(69), not null)
                no_of_copies(int, not null)
                cost(int, not null)

    StudentTable :  id(varchar(69), not null)
                    sname(varchar(69), not null)
                    pswd(varchar(69), not null)
                    booktaken(varchar(69), not null)
                    returntime(int, not null)
*/