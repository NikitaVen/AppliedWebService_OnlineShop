CREATE TABLE Manufacturer
(
    ID          BIGINT PRIMARY KEY IDENTITY (1, 1),
    Title       NVARCHAR(30) NOT NULL UNIQUE,
    Description NVARCHAR(150)
);

INSERT INTO Manufacturer (Title, Description)
VALUES ('HP', NULL),
       ('Dell', NULL),
       ('Apple', NULL),
       ('Asus', NULL),
       ('Samsung', NULL),
       ('Lenovo', NULL);

INSERT INTO Manufacturer (Title, Description)
VALUES ('DeepCool', NULL);

INSERT INTO Manufacturer (Title, Description)
VALUES ('Keychron', NULL);

INSERT INTO Manufacturer (Title, Description)
VALUES ('Huawei', NULL);

INSERT INTO Manufacturer (Title, Description)
VALUES ('Razer', NULL);

INSERT INTO Manufacturer (Title, Description)
VALUES ('MSI', NULL);

INSERT INTO Manufacturer (Title, Description)
VALUES ('Kingston', NULL);

INSERT INTO Manufacturer (Title, Description)
VALUES ('Intel', NULL);

CREATE TABLE Item_code
(
    Code  BIGINT PRIMARY KEY IDENTITY (1, 1),
    Title NVARCHAR(50) NOT NULL
);

INSERT INTO Item_code (Title)
VALUES ('Laptop'),
       ('Monitor'),
       ('SSD'),
       ('HDD'),
       ('Power unit'),
       ('GPU'),
       ('CPU'),
       ('Keyboard'),
       ('Mouse'),
       ('PC'),
       ('RAM'),
       ('CD ROM');

INSERT INTO Item_code (Title)
VALUES ('RAM');

CREATE TABLE Item
(
    ID              BIGINT PRIMARY KEY IDENTITY (1, 1),
    Title           NVARCHAR(30) NOT NULL,
    Item_code       BIGINT       NOT NULL FOREIGN KEY (Item_code) REFERENCES Item_code (Code),
    ID_Manufacturer BIGINT       NOT NULL FOREIGN KEY (ID_Manufacturer) REFERENCES Manufacturer (ID),
    Image           VARBINARY(MAX),
    Description     NVARCHAR(300),
    Amount          BIGINT       NOT NULL
);

ALTER TABLE Item
    ADD Price MONEY NOT NULL;

DROP TABLE COMPUTER;

CREATE FUNCTION dbo.ChkValidEmail(@EMAIL NVARCHAR(100)) RETURNS bit as
BEGIN
    DECLARE @bitEmailVal as Bit
    DECLARE @EmailText nvarchar(100)

    SET @EmailText = ltrim(rtrim(isnull(@EMAIL, '')))

    SET @bitEmailVal = case
                           when @EmailText = '' then 0
                           when @EmailText like '% %' then 0
                           when @EmailText like ('%["(),:;<>\]%') then 0
                           when substring(@EmailText, charindex('@', @EmailText), len(@EmailText)) like
                                ('%[!#$%&*+/=?^`_{|]%') then 0
                           when (left(@EmailText, 1) like ('[-_.+]') or right(@EmailText, 1) like ('[-_.+]')) then 0
                           when (@EmailText like '%[%' or @EmailText like '%]%') then 0
                           when @EmailText LIKE '%@%@%' then 0
                           when @EmailText NOT LIKE '_%@_%._%' then 0
                           else 1
        end
    RETURN @bitEmailVal
END;


CREATE TABLE Ord
(
    ID         BIGINT PRIMARY KEY IDENTITY (1, 1),
    Order_date DATETIME NOT NULL,
    Email      NVARCHAR(50) CHECK (dbo.ChkValidEmail(Email) = 1),
    Address    NVARCHAR(100)
);

CREATE TABLE Ord_Item
(
    ID_Ord  BIGINT FOREIGN KEY (ID_Ord) REFERENCES Ord (ID),
    ID_Item BIGINT FOREIGN KEY (ID_Item) REFERENCES Item (ID),
    Amount  BIGINT,
    CONSTRAINT Ord_Item_PK PRIMARY KEY (ID_Ord, ID_Item),
);

INSERT INTO Item(Title, Item_code, ID_Manufacturer, Image, Description, Amount, Price)
VALUES ('PK800D', 5, 7,
        (SELECT BulkColumn FROM OPENROWSET(BULK 'D:\3 course\ASP\lab3\pics\bp.jpeg', SINGLE_BLOB) AS Image),
        N'800 Вт, активная PFC, КПД 85%б 12V 66.5 A, бронзовый сертификат', 10, 204.07);

INSERT INTO Item(Title, Item_code, ID_Manufacturer, Image, Description, Amount, Price)
VALUES ('K3D3', 8, 8,
        (SELECT BulkColumn FROM OPENROWSET(BULK 'D:\3 course\ASP\lab3\pics\kb.png', SINGLE_BLOB) AS Image),
        N'Тип: механическая, Материал: пластик, алюминий, Интерфейс: провод, bluetooth, Переключатели: Brown Switch',
        15, 575.12);

INSERT INTO Item(Title, Item_code, ID_Manufacturer, Image, Description, Amount, Price)
VALUES ('MateBook D15', 1, 9,
        (SELECT BulkColumn FROM OPENROWSET(BULK 'D:\3 course\ASP\lab3\pics\lt.jpg', SINGLE_BLOB) AS Image),
        N'Оперативная память: 8ГБ, Процессор: Intel Core-i3 115G4, Цвет: серебристый, Видеокарта: UHD Graphics, SSD: 256ГБ',
        8, 2099.00);

INSERT INTO Item(Title, Item_code, ID_Manufacturer, Image, Description, Amount, Price)
VALUES ('Victus 15', 1, 1,
        (SELECT BulkColumn FROM OPENROWSET(BULK 'D:\3 course\ASP\lab3\pics\victus.jpg', SINGLE_BLOB) AS Image),
        N'Экран: 15.6, FULLHD, IPS, 144 Гц, Процессор: Intel Core-i5 12450H, Видеокарта: GeForce GTX 1650', 11,
        3399.00);

INSERT INTO Item(Title, Item_code, ID_Manufacturer, Image, Description, Amount, Price)
VALUES ('Victus 15', 9, 10,
        (SELECT BulkColumn FROM OPENROWSET(BULK 'D:\3 course\ASP\lab3\pics\rzrmouse.jpg', SINGLE_BLOB) AS Image),
        N'Тип: игровая, сенсор: оптический, количество кнопок: 5, цвет: черный, тип соединения: проводная', 35, 89.00);

INSERT INTO Item(Title, Item_code, ID_Manufacturer, Image, Description, Amount, Price)
VALUES ('870 Evo 500GB', 3, 5,
        (SELECT BulkColumn FROM OPENROWSET(BULK 'D:\3 course\ASP\lab3\pics\ssdSamsung.jpg', SINGLE_BLOB) AS Image),
        N'Скорость чтения: 560 МБ/с, Скорость записи: 530 МБ/с, Средняя наработка: 1500000 ч, Интерфейс: SATA 3.0', 15,
        245.54);

INSERT INTO Item(Title, Item_code, ID_Manufacturer, Image, Description, Amount, Price)
VALUES ('VP229HE', 2, 4,
        (SELECT BulkColumn FROM OPENROWSET(BULK 'D:\3 course\ASP\lab3\pics\asusm.jpg', SINGLE_BLOB) AS Image),
        N'Диагональ: 21.5, Тип: IPS, Разрешение: FULLHD, Частота: 60 Гц, Время отклика: 5 мс', 19, 459.00);

INSERT INTO Item(Title, Item_code, ID_Manufacturer, Image, Description, Amount, Price)
VALUES ('GeForce RTX 2060 Super Gaming', 6, 11,
        (SELECT BulkColumn FROM OPENROWSET(BULK 'D:\3 course\ASP\lab3\pics\rtx2060.jpg', SINGLE_BLOB) AS Image),
        N'Тип видеопамяти: GDDR6, Объем видеопамяти: 8ГБ, Поддержка DirectX: 12', 7, 1399.00);


INSERT INTO Item(Title, Item_code, ID_Manufacturer, Image, Description, Amount, Price)
VALUES ('ValueRAM 16GB', 13, 12,
        (SELECT BulkColumn FROM OPENROWSET(BULK 'D:\3 course\ASP\lab3\pics\ram.jpg', SINGLE_BLOB) AS Image),
        N'Тип: DDR4 DIMM, Объем: 16 ГБ, Частота: 3200 МГц, Напряжение: 1.2 В', 8, 247.00);

INSERT INTO Item(Title, Item_code, ID_Manufacturer, Image, Description, Amount, Price)
VALUES ('Core i3-10100', 7, 13,
        (SELECT BulkColumn FROM OPENROWSET(BULK 'D:\3 course\ASP\lab3\pics\corei3.jpg', SINGLE_BLOB) AS Image),
        N'Серия: Core i3, Модель: G4900, Количество ядер: 4, Тактовая частота: 3.6 ГГц, Встроенная частота: 4.3 ГГц', 5,
        450.00);

CREATE OR ALTER TRIGGER on_order_item_insert
    ON Ord_Item
    AFTER INSERT
    AS
    UPDATE Item
    SET Amount = Amount - (SELECT Amount FROM inserted WHERE ID_Item = ID)
    WHERE ID in (SELECT ID_Item FROM inserted)