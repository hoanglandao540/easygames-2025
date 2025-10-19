# EasyGames (ASP.NET Core MVC, .NET 8, SQLite)

A small web app for a *games/toys/books* store. It supports three roles:

- *Customer* – browse, search/filter, cart, checkout  
- *Shop (Proprietor)* – POS sell items; stock goes down safely  
- *Owner* – manage users, shops, and shop stocks; (scaffold) email + tiers

Theme: *Midnight + Gold*.  
Architecture: *MVC + simple DDD layering*.

---

## Features

*Customer*
- Catalog with *Search* and *Filters*
- *Cart*: +/– quantity, Remove, Clear on the *same page*
- *Checkout* with a clear success page

*Shop (Proprietor)*
- *POS: add lines and **Pay*
- Stock decreases safely (*no negative quantity* rule)

*Owner*
- *Users* admin (create/edit/delete with roles)
- *Shops* admin and *ShopStocks* view/adjust
- (Scaffold) *Email* + *Tier* services for later

---

## Tech Stack

- *.NET 8, **ASP.NET Core MVC*
- *Entity Framework Core* (SQLite)
- Unit Testing: *xUnit* (or MSTest, based on your solution)
- Razor Views, Bootstrap/Tailwind (depending on your setup)

