using System;
using Dapper;
using MySql.Data.MySqlClient;
using NUnit.Framework;

namespace Test
{
	[TestFixture]
	public class TableOperationTests
	{
		private static string _connectionString = "server=localhost;user=user;password=password;database=test;";
		MySqlConnection cn;

		class UserDB : Database<UserDB>
		{
			public Table<User> Users { get; set; }
		}
			
		class User {
			[PrimaryKey]
			public int Id { get; set; }
		}
			
		[SetUp]
		public void Setup()
		{            
			cn = new MySqlConnection(_connectionString);
			cn.Open();
			db = UserDB.Init(cn, 30);
		}

		[TearDown]
		public void Teardown(){
			cn.Close ();
		}

		UserDB db;
		[Test]
		public void CreateTable(){
			db.Execute ("drop table if exists user;");
			db.Users.Create ();
			var rows = db.Query("SHOW INDEXES FROM user WHERE Key_name = 'PRIMARY'");
			Assert.IsTrue (rows.AsList ().Count == 1);
			db.Execute ("drop table if exists user;");
		}

		[Test]
		public void DeleteTable(){
			db.Users.Drop ();
		}

		[Test]
		public void TableAlreadyExistsExceptionTest(){
			db.Users.Create ();
			Assert.Throws<TableAlreadyExistsException>(db.Users.Create);
		}
	}

}

