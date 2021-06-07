use StudyApp
go

insert into subject(Subject_Name)values('TV Shows');
insert into subject(Subject_Name)values('Programmierung');
insert into subject(Subject_Name)values('BWL');
insert into Subject(Subject_Name)values('Filme');
insert into Subject(Subject_Name)values('Musik');
insert into Subject(Subject_Name)values('Deutsch');
insert into Subject(Subject_Name)values('Englisch');
insert into Subject(Subject_Name)values('Französisch');
insert into Subject(Subject_Name)values('Biologie');


insert into [User]([User_Name],Hash,Salt)values('a','sio5738hYXGDN5E2u7QspCsgLI23Dm0xhJBdu/TakCA=','cYUTWAMkYcM=');
insert into [User]([User_Name],Hash,Salt)values('c','aRnnzYvmnXco+/nNiMJ2OY7xHaMPmq5RSxREoTBtVTw=','ikCssgDUGgw=');
insert into [User]([User_Name],Hash,Salt)values('g','X2XJ7tZUeeiOX0QcLLUOywccwiEwq2tf4aAggubErvc=','e0yG6jNhCK4=');

insert into item(Item_Name, Item_Content, Subject_ID) values('The X-Files', 'TrustNo1', 1);
insert into item(Item_Name, Item_Content, Subject_ID) values('Star Trek', 'Live Long And Prosper', 1);
insert into item(Item_Name, Item_Content, Subject_ID) values('House MD', 'Everybody Lies', 1);
insert into item(Item_Name, Item_Content, Subject_ID) values('Game of Thrones', 'Valar Morghulis', 1);
insert into item(Item_Name, Item_Content, Subject_ID) values('Game of Thrones', 'Not Today', 1);
insert into item(Item_Name, Item_Content, Subject_ID) values('The 100','Jus Drein Jus Daun', 1);
insert into Item(Item_Name, Item_Content, Subject_ID)values('Malapert','frech',7);
insert into Item(Item_Name, Item_Content, Subject_ID)values('Grandiloquent','übertrieben',7);
insert into Item(Item_Name, Item_Content, Subject_ID)values('Flibbertigibbet','Luftikus',7);
insert into Item(Item_Name, Item_Content, Subject_ID)values('Curmudgeon','Griesgram',7);
insert into Item(Item_Name, Item_Content, Subject_ID)values('Sesquipedalian','schwülstig',7);
insert into Item(Item_Name, Item_Content, Subject_ID)values('Blatherskite','Termin',7);

insert into Session(Session_Date,Session_Points,User_ID)values('2021-02-06','20',1);
insert into Session(Session_Date,Session_Points,User_ID)values('2021-02-06','45',1);
insert into Session(Session_Date,Session_Points,User_ID)values('2021-02-06','82',1);
insert into Session(Session_Date,Session_Points,User_ID)values('2021-02-06','17',1);
insert into Session(Session_Date,Session_Points,User_ID)values('2021-02-06','33',1);
insert into Session(Session_Date,Session_Points,User_ID)values('2021-02-06','53',1);

insert into ItemSession(Item_ID,Session_ID)values(1,3);
insert into ItemSession(Item_ID,Session_ID)values(2,3);
insert into ItemSession(Item_ID,Session_ID)values(5,1);
insert into ItemSession(Item_ID,Session_ID)values(3,3);
insert into ItemSession(Item_ID,Session_ID)values(1,2);
insert into ItemSession(Item_ID,Session_ID)values(4,2);




