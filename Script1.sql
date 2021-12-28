drop table [User];
create table [User] (
	IdUser uniqueidentifier not null primary key,
	IdRole uniqueidentifier not null default '4EA83F97-6116-44E2-B6EB-437A3BE9C12C',
	UserName nvarchar(200) not null,
	FirstName nvarchar(200) not null,
	LastName nvarchar(200) not null,
	Email nvarchar(200) not null unique,
	Password nvarchar(500) not null,
	UserImage varbinary(max),
	Alive bit default 1,
);


create table [RegularUser] (
	IdRegularUser uniqueidentifier not null primary key,
	Height int,
	Weight int,
	Gender int,
	Streak int default 0,
	BirthDate date,
	constraint FK_UserRegularUser foreign key (IdRegularUser)
	references [User](IdUser),
	constraint CK_ValidGender check (Gender > 0 and Gender < 5)
);

create table [Role] (
	IdRole uniqueidentifier not null primary key,
	Name nvarchar(200) not null,
	Description nvarchar(max) not null,
);

insert into [Role] values ('4EA83F97-6116-44E2-B6EB-437A3BE9C12C', 'Regular User', 'Has no special ability')
insert into [Role] values (NEWID(), 'Admin', 'Is allowed to delete accounts');
insert into [Role] values (NEWID(), 'Trainer', 'Is allowed to to add, edit and delete workouts and programs');
insert into [Role] values (NEWID(), 'Nutritionist', 'Is allowed to add, edit and delete aliments');


create table [SpecialUser] (
	IdSpecialUser uniqueidentifier not null primary key,
	IdRole uniqueidentifier not null,
	ShortDescription nvarchar(max),
	LongDescription nvarchar(max),
	constraint FK_UserSpecialUser foreign key (IdSpecialUser)
	references [User](IdUser),
	constraint FK_RoleSpecialUser foreign key (IdRole)
	references [Role](IdRole) 
);

drop table Request
create table [Request] (
	IdFromUser uniqueidentifier not null,
	IdToUser uniqueidentifier not null,
	Status int not null,
	Date date not null,
	primary key (IdFromUser, IdToUser),
	constraint FK_RequestFromRegularUser foreign key (IdFromUser)
	references [RegularUser](IdRegularUser),
	constraint FK_RequestToRegularUser foreign key (IdToUser)
	references [regularUser](IdRegularUser),
);

create table [FriendsList] (
	IdUser1 uniqueidentifier not null,
	IdUser2 uniqueidentifier not null,
	constraint FK_FriendsListUser1 foreign key (IdUser1)
	references [RegularUser](IdRegularUser),
	constraint FK_FriendsListUser2 foreign key (IdUser2)
	references [RegularUser](IdRegularUser),
);

create table [CategoryW] (
	IdCategory uniqueidentifier not null primary key,
	Name nvarchar(200) not null,
);

create table [Workout] (
	IdWorkout uniqueidentifier not null primary key,
	Name nvarchar(200) not null,
	Description nvarchar(200) not null,
	Image varbinary(max),
	Time int,
	IdTrainer uniqueidentifier,
	IdLastModified uniqueidentifier,
	LinkUrl nvarchar(500) not null,
	Calories int not null,

	IdCategory uniqueidentifier not null,
	constraint FK_WorkoutTrainer foreign key (IdTrainer)
	references [SpecialUser](IdSpecialUser),
	constraint FK_WorkoutLastModified foreign key (IdLastModified)
	references [SpecialUser](IdSpecialUser),
);

CREATE TABLE [Workout-Category] (
	IdWorkout uniqueidentifier not null,
	IdCategory uniqueidentifier not null,

	primary key (IdWorkout, IdCategory),
	constraint FK_Category foreign key (IdCategory)
	references [CategoryW](IdCategory),
	constraint FK_Workout foreign key (IdWorkout)
	references [Workout](IdWorkout)
);

create table [FitProProgram] (
	IdProgram uniqueidentifier not null primary key,
	TimePeriod int not null
);

create table [FitProProgram-Workout] (
	IdProgram uniqueidentifier not null,
	IdWorkout uniqueidentifier not null,
	DayNumber int not null
	primary key (IdWorkout, IdProgram, DayNumber),
	constraint FK_FPW_Program foreign key (IdProgram)
	references [FitProProgram](IdProgram),
	constraint FK_PW_Workout foreign key (IdWorkout)
	references [Workout](IdWorkout)
);

create table [FitProProgram-Category] (
	IdProgram uniqueidentifier not null,
	IdCategory uniqueidentifier not null,
	primary key (IdCategory, IdProgram),
	constraint FK_FPPC_Program foreign key (IdProgram)
	references [FitProProgram](IdProgram),
	constraint FK_FPPC_Category foreign key (IdCategory)
	references [CategoryW](Idcategory)
);

create table [RegularUser-FitProProgram] (
	IdProgram uniqueidentifier not null,
	IdRegularUser uniqueidentifier not null,
	StartDate date not null,
	CurrentDay int not null default 0,
	primary key (IdRegularUser, IdProgram),
	constraint FK_RUFP_Program foreign key (IdProgram)
	references [FitProProgram](IdProgram),
	constraint FK_RUP_RegularUser foreign key (IdRegularUser)
	references [RegularUser](IdRegularUser)
);

drop table Aliment;
create table [Aliment] (
	IdAliment uniqueidentifier not null primary key,
	IdNutritionist uniqueidentifier not null,
	Name varchar(max) not null,
	Calories float not null,
	Protein float not null,
	Fat float not null,
	Carbo float not null, 
	
	constraint FK_AlimentNutritionist foreign key (IdNutritionist)
	references [SpecialUser](IdSpecialUser)
);

drop table [Aliment-RegularUser]
create table [Aliment-RegularUser] (
	IdAliment uniqueidentifier not null,
	IdRegularUser uniqueidentifier not null,
	Date datetime not null,
	Quantity int not null,
	primary key (IdRegularUser, IdAliment, Date),
	constraint FK_ARU_Program foreign key (IdAliment)
	references [Aliment](IdAliment),
	constraint FK_ARU_RegularUser foreign key (IdRegularUser)
	references [RegularUser](IdRegularUser)
);

create table [CategoryR] (
	IdCategory uniqueidentifier not null primary key,
	Name nvarchar(200) not null
);

create table [Recipe] (
	IdRecipe uniqueidentifier not null primary key,
	Name nvarchar(300) not null,
	AlimentsList nvarchar(max) not null,
	Preparation nvarchar(max) not null,
	Time int not null,
	Calories int not null,
	Image varbinary(max) not null,
	IdCategory uniqueidentifier,
	IdNutritionist uniqueidentifier
	
	constraint FK_RecipeCategory foreign key (IdCategory)
	references [CategoryR](IdCategory),
	constraint FK_RecipeNutritionist foreign key (IdNutritionist)
	references [SpecialUser](IdSpecialUser)
);

create table [Aliment-Recipe](
	IdAliment uniqueidentifier not null,
	IdRecipe uniqueidentifier not null,
	Quantity int not null,

	primary key (IdAliment, IdRecipe),
	constraint FK_AR_Aliment foreign key (IdAliment)
	references [Aliment](IdAliment),
	constraint FK_AR_Recipe foreign key (IdRecipe)
	references [Recipe](IdRecipe)
)

create table [Recommandation] (
	Idrecommandation uniqueidentifier not null primary key,
	IdFromUser uniqueidentifier not null,
	IdToUser uniqueidentifier not null,
	IdWorkout uniqueidentifier,
	IdRecipe uniqueidentifier,
	SendDate Date not null,
	Comment nvarchar(500),

	constraint FK_RecommandationFromRegularUser foreign key (IdFromUser)
	references [RegularUser](IdRegularUser),

	constraint FK_RecommandationToRegularUser foreign key (IdToUser)
	references [RegularUser](IdRegularUser),
	
	constraint FK_RecommandationWorkout foreign key (IdWorkout)
	references [Workout](IdWorkout),	

	constraint FK_RecommandationRecipe foreign key (IdRecipe)
	references [Recipe](IdRecipe),

	constraint UK_Recommandation unique (IdFromUser, IdToUser, IdWorkout, IdRecipe, SendDate),
);

create table Saved (
IdSaved uniqueidentifier not null primary key,
IdRegularUser uniqueidentifier not null,
IdWorkout uniqueidentifier,
IdRecipe uniqueidentifier,
Date date,

constraint FK_Saved_RegularUser foreign key (IdRegularUser)
	references [RegularUser](IdRegularUser),

constraint FK_Saved_Recipe foreign key (IdRecipe)
	references [Recipe](IdRecipe),
	
constraint FK_Saved_Workout foreign key (IdWorkout)
	references [Workout](IdWorkout),

constraint UK_Saved unique (IdRegularUser, IdWorkout, IdRecipe),

);

create table UserActiveDays (
Date date not null,
IdRegularUser uniqueidentifier not null,

constraint FK_UserActiveDays_RegularUser foreign key (IdRegularUser)
	references [RegularUser](IdRegularUser),
primary key (IdRegularUser, Date),
);