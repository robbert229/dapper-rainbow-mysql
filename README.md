Dapper.Rainbow.MySql
=======================

This project is a reimplementation of Dapper.Rainbow designed for MySql. It is an addon that gives you basic crud operations while having to write even less sql.
[![Build status](https://ci.appveyor.com/api/projects/status/avtttl8f5kiirsr4?svg=true)](https://ci.appveyor.com/project/robbert229/dapper-rainbow-mysql)


    class User {
      [PrimaryKey]
      public int Id { get; set; }
      [NotNull]
      public String Email { get; set; }
      [NotNull]
      public String Password { get; set; }
      public String Name { get; set; }
    }
    
    class UserDB : Database<UserDB> {
      public Table<User> Users { get; set; }
    }
    
    class Demo {
      public void Do(){
        using(var conn = new MysqlConnection(connectionString)){
          conn.Open();
          var db = UserDB.Init(conn, commandTimeout: 2);
          
          //drop the table if it exists
          db.Execute ("drop table if exists user;");
          
          try {
            db.Users.Create()
          } catch (TableAlreadyExistsException){ }
          
          /*  
            Do somthing interesting in here 
          */
        }
      }
    }


How it finds the tables
------------
Dapper.Rainbow.MySql knows what table to query based on the name of the class. 
In this situation the table that Rainbow looks in is the User table. It is not
pluralized. 


Table Creation
----------
dapper-rainbow-mysql also has functionality that doesn't exist in dapper-rainbow.
dapper-rainbow-mysql has the ability to intelligently create a table from your 
POCOs (Plain Old Clr Object) automatically. You add constraints and modifiers 
through attributes.

**WARNING** currently automatic table creation is in early beta. It doesn't support
all types and will through an exception if you try to use somthing that it is not
familiar. It also doesn't allow any properties to be non primitive excluding DateTime.


### To Add A Primary Key Constraint
    
    class {
        [PrimaryKey]
        int Id { get; set; }
    }

### To Add A Not Null Constraint

    class {
        [NotNull]
        string Name { get; set; }
    }
    
### To Add Auto Increment
    
    class {
        [AutoIncrement]
        int customerNumber { get; set; }
    }

### Create Table From Model ~ Throws exception if table already exists
    db.Users.Create()

### Create Table From Model
    db.Users.TryCreate()

### Drop Table
    db.Users.Drop()


Model Interaction
----------
### Get All The Users
    IEnumerable<User> all = db.Users.All();
    
### Get A User
    User user = db.Users.Get(userId);
    User same_user = db.Users.Get(new {Id = userId});

### Delete a User 
    bool isTrue = db.Users.Delete(user);
    bool isAnotherTrue = db.Users.Delete(new {Id = userId});
  
### Get The First User
    User user = db.Users.First();
  
### Insert A User
    long uid = db.Users.Insert (
        new { Email="foolio@coolio.com", 
              Name="Foolio Coolio", 
              Password="AHashedPasswordOfLengthThirtyTwo"});

### Insert Or Update A User
    int uid = db.Users.InsertOrUpdate(user);
    
### Update
    user.Name = "Foolio Jr."
    int uid = db.Users.Update(uid, user);
    int uid = db.Users.Update(new {Id = uid}, user);
