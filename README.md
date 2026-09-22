Expense and Budget Tracke
Expense & Budget Tracker — 1. اﻟﻔﻛرة اﻷﺳﺎﺳﯾﺔاﻟﻔﻛرة ﺑﺑﺳﺎطﺔ إﻧﻧﺎ ھﻧﻌﻣل Backend API ﯾﺳﻣﺢ ﻟﻠﻣﺳﺗﺧدم إﻧﮫ ﯾدﯾر ﺣﺎﻟﺗﮫ اﻟﻣﺎﻟﯾﺔ.
ﯾﻌﻧﻲ اﻟﻣﺳﺗﺧدم ﯾﻘدر:

.Register / Login ﯾﻌﻣلﯾﺿﯾف دﺧﻠﮫ.
ﯾﺿﯾف ﻣﺻﺎرﯾﻔﮫ.
ﯾﻌﻣل Categories ﺧﺎﺻﺔ ﺑﯾﮫ.
ﯾﺣدد Budget ﻟﻛل Category ﻛل ﺷﮭر.
ﯾﺿﯾف ﻣﺻﺎرﯾف ﻣﺗﻛررة.
اﻟﻧظﺎم ﯾوﻟ ّ د اﻟﻣﺻﺎرﯾف اﻟﻣﺗﻛررة ﺗﻠﻘﺎﺋﯾًﺎ.
اﻟﻧظﺎم ﯾراﺟﻊ اﻟـ Budgets ﯾوﻣﯾًﺎ.

.Email أو ﺗﻌدّاه، ﯾﺑﻌث Budget ﻟو اﻟﻣﺳﺗﺧدم ﻗرب ﻣنﯾطﻠب Reports ﻋن ﺷﮭر ﻣﻌﯾن أو ﯾﻘﺎرن ﺑﯾن ﺷﮭور.

Users — اﻟﻣﺳﺗﺧدﻣﯾن
.Multi-user اﻟﻣﺷروعﯾﻌﻧﻲ ﻗﺎﻋدة اﻟﺑﯾﺎﻧﺎت ﻣﻣﻛن ﯾﻛون ﻓﯾﮭﺎ:

Ahmed
Mohamed
Omar
Ali
...وﻛل واﺣد ﻟﮫ ﺑﯾﺎﻧﺎﺗﮫ اﻟﺧﺎﺻﺔ.
ﻣﺛﻼً:

Ahmed
├── Categories
├── Transactions
├── Budgets
└── RecurringTransactions
وﻣﺣﻣد
:
Mohamed
├── Categories
├── Transactions
├── Budgets
└── RecurringTransactions
3. Authentication و Authorization
User
Adminأھم ﻧﻘطﺔ
ﻻزم ﻧﺿﻣن إن أﺣﻣد ﻣﯾﻧﻔﻌش ﯾﺷوف ﺑﯾﺎﻧﺎت ﻣﺣﻣد.

:Users ﻋﻧدﻧﺎ ﻧوﻋﯾن
User
Categories
Transactions
Budgets
Recurring Transactions
Reportsاﻟﻣﺳﺗﺧدم اﻟﻌﺎدي ﯾﻘدر ﯾدﯾر ﺑﯾﺎﻧﺎﺗﮫ:
ﻟﻛن ﻓﻘط ﺑﯾﺎﻧﺎﺗﮫ ھو.

Admin
GET /api/admin/usersاﻟـ Admin ﻋﻧده ﺻﻼﺣﯾﺎت أﻋﻠﻰ.
ﻣﺛﻼً:
ﯾﻘدر ﯾﺷوف اﻟﻣﺳﺗﺧدﻣﯾن.

وﻛﻣﺎن
:
Activate User
Deactivate Userﯾﻌﻧﻲ:

Ahmed → Active
Mohamed → Disabled
Omar → Active
:Login ﺣﺎول ﯾﻌﻣل Mohamed ﻟو
POST /api/auth/loginاﻟـ API ﯾﺗﺄﻛد:

IsActive == false
4. JWT Authentication
Register
↓
Login
↓
Validate Email/Password
↓
Generate JWT
↓
Client receives Token
.JWT ھﻧﺳﺗﺧدماﻟـ Flow ھﯾﻛون ﺗﻘرﯾﺑًﺎ:
ﺑﻌد ﻛده أي Request ﻣﺣﻣﻲ:

Authorization: Bearer eyJ...اﻟـ API ﯾﻘرأ اﻟـ JWT وﯾﻌرف:

UserId = 123
Role = User
User.FindFirst(ClaimTypes.NameIdentifier)
.Request وﺑﺎﻟﺗﺎﻟﻲ ﻧﻘدر ﻧﻌرف ﺻﺎﺣب اﻟـﻣﺛﻼً:
وﻣن اﻟـ UserId ده ﻧﺟﯾب ﺑﯾﺎﻧﺎﺗﮫ ﻓﻘط

Currency 5. اﻟﻌﻣﻠﺔاﻟﻣﺷروع ﯾدﻋم ﻋﻣﻠﺔ واﺣدة ﻓﻘط.
ﻣﺛﻼً:

EGPوده ﻗرار ﻣﮭم ﺟدًا ﻷﻧﮫ ﺑﯾﺑﺳّط اﻟﻣﺷروع.

6. Categories
Food
Transportation
Rent
Entertainment
Salary
Freelance
.Transaction ھﻲ اﻟﺗﺻﻧﯾف اﻟﻠﻲ ﺑﻧﺣط ﺗﺣﺗﮫ اﻟـ Category اﻟـﻣﺛﻼ ً اﻟﻣﺳﺗﺧدم ﻣﻣﻛن ﯾﻌﻣل:
ﻟﻛن ﻋﻧدﻧﺎ ﻧﻘطﺔ ﻣﮭﻣﺔ ﺟدًا:
ﻛل User ﯾﻌﻣل Categories ﺑﻧﻔﺳﮫ.
ﯾﻌﻧﻲ ﻣﻔﯾش:

System Categoriesﻣﺛﻼً:

Ahmed:
Food
Rent
Salary
Mohamed:
Food
Gaming
Freelance
7. Income Category و Expense Category
Incomeﻛل واﺣد ﻣﺳﺗﻘل.
ﻛل Category ﻻزم ﻧﻌرف ھﻲ ﺧﺎﺻﺔ ﺑـ:
أو:

Expenseﻣﺛﻼً:

Salary → Income
Freelance → Incomeﺑﯾﻧﻣﺎ:

Food → Expense
Rent → Expense
Transportation → Expenseﻟﯾﮫ؟
ﻋﺷﺎن ﻟﻣﺎ ﻧﻌﻣل Report ﻧﻘدر ﻧﺣﺳب:

Total Income
Total Expense
Net
ﻣﺛﻼً
:
Income:
Salary = 15000
Freelance = 2000
Total Income = 17000
Expense:
Food = 2000
Rent = 5000
Transportation = 1000و:

Total Expense = 8000
Net = Income - Expense
Net = 17000 - 8000
Net = 9000إذن:

8. Transactions أﻛﻞ
 200 ﺟﻨﯿ
ﮫدﻓﻌ
ﺖاﻟـ Transaction ھﻲ ﺣرﻛﺔ ﻣﺎﻟﯾﺔ ﻓﻌﻠﯾﺔ ﺣﺻﻠت.
ﻣﺛﻼً:

.Transaction ديأو:
 ﻣﺮﺗﺐ
 15000 ﺟﻨﯿ
ﮫاﺳﺘﻠﻤ
ﺖ
9. Manual Transactionدي Transaction ﺑرﺿﮫ.

دي أﺑﺳط ﻧوع
.اﻟﻣﺳﺗﺧدم ﯾدﺧﻠﮭﺎ ﺑﻧﻔﺳﮫ.
ﻣﺛﻼً:

POST /api/transactionsوﯾرﺳل:

{
}
"categoryId": 5,
"amount": 250,
"date": "2026-09-22",
"description": "Lunch"اﻟـ Backend ﯾﺳﺟل:

Transaction----------------
UserId
CategoryId
Amount
Date
Description
10. Recurring Transactions
Rentدي Transactions ﺑﺗﺗﻛرر.
ﻣﺛﻼً:
ﻛل ﺷﮭر:

1 September
1 October
1 November
1 December
...
ﺑدل ﻣﺎ اﻟﻣﺳﺗﺧدم ﯾدﺧﻠﮭﺎ ﻛل ﺷﮭر، ﯾﻌﻣﻠﮭﺎ ﻣرة واﺣدة
:
Recurring Transaction--------------------
Name: Rent
Amount: 5000
Category: Rent
StartDate: 2026-09-01
NextRunDate: 2026-10-01وﺑﻌد ﻛده اﻟﻧظﺎم ﯾﺗوﻟﻰ اﻟﺑﺎﻗﻲ.

12. Hangfire
Hangfire ﻣﺳؤول ﻋن ﺗﺷﻐﯾل Background Jobs.
GenerateRecurringTransactionsJob
.Hangfire ھﻧﺎ ﯾﺄﺗﻲ دور
:Job ﻣﺛﻼ ً ﻋﻧدﻧﺎﯾﺗﻧﻔذ ﯾوﻣﯾًﺎ.
ﻣﺛﻼ ً اﻟﺳﺎﻋﺔ:

00:00ﯾﻌﻣل:

Find recurring transactions
↓
Is NextRunDate <= Today?
↓
Yes
↓
Create Transaction
↓
Calculate next run date
↓
Save13. ﻟﯾﮫ Job ﯾوﻣﻲ؟

ﻣﻣﻛن ﻧﺳﺄل
:ﻟﯾﮫ ﻣش ﻧﺧﻠﻲ Hangfire ﯾﺷﻐل Job ﻟﻛل Transaction؟
ﻣﻣﻛن، ﻟﻛن اﻟﺗﺻﻣﯾم اﻟﺣﺎﻟﻲ أﺑﺳط.
ﺑدل ﻣﺎ ﯾﻛون ﻋﻧدﻧﺎ:

Job 1 → Rent
Job 2 → Netflix
Job 3 → Salary
Job 4 → Internet
...ﻧﺧﻠﻲ ﻋﻧدﻧﺎ Job واﺣد ﯾوﻣﻲ:

DailyRecurringTransactionJob
.recurring transactions ﯾﻔﺣص ﻛل اﻟـوده أﺳﮭل ﻓﻲ اﻹدارة.

14. Budgets
September 2026
Food            
→ 2500
Transportation  → 1000
Entertainment   
Rent            اﻟـ Budget ھو اﻟﺣد اﻟﻠﻲ اﻟﻣﺳﺗﺧدم ﻣﺣدده ﻟﻠﻣﺻﺎرﯾف
.ﻟﻛن ﻣش Budget واﺣد ﻟﻛل اﻟﺷﮭر.
ﻛل Category ﻟﮭﺎ Budget ﻣﺳﺗﻘل.
ﻣﺛﻼً:

→ 1500
→ 5000
15. Budget ﻣرﺗﺑط ﺑﺷﮭر وﺳﻧﺔاﻟـ Budget ﻻزم ﯾﻛون ﻣرﺑوط ﺑـ:

Month
Year
Category
User
Amount
User: Ahmed
Category: Food
Month: 9
Year: 2026
Amount: 2500ﻣﺛﻼً:
وﻓﻲ أﻛﺗوﺑر ﻣﻣﻛن ﯾﻐﯾره:

Food
October 2026
Budget = 3000إذن:

September → 2500
October   
→ 3000
November  → 2800
16. Notifications
Food Budget = 2500
Spent = 2200
.Core Features ﻣن اﻟـ Notifications اﻟـاﻟﮭدف إن اﻟﻧظﺎم ﯾﻘول ﻟﻠﻣﺳﺗﺧدم:
أﻧت ﻗرﺑت ﻣن Budget ﺑﺗﺎع Category ﻣﻌﯾﻧﺔ.
ﻣﺛﻼً:
اﻟﻧﺳﺑﺔ:

2200 / 2500 = 88%
You have used 88% of your Food budget.ﻓﻧﻘدر ﻧﺑﻌث:
وﻟو:

Spent = 2700
Budget = 2500ﯾﺑﻘﻰ:

You exceeded your Food budget by 200 EGP.17. اﻟﺗﻧﺑﯾﮭﺎت Email ﻓﻘط
ﻣش ھﻧﺳﺗﺧدم:

Push Notification
SMS
SignalR
WebSocket
Real-time 18. اﻟﺗﻧﺑﯾﮫ ﻣشدي ﻧﻘطﺔ ﻣﮭﻣﺔ.

:Transaction ﻟو اﻟﻣﺳﺗﺧدم ﻋﻣل
Food = 300ﻣش ﻻزم ﻓورً ا ﯾﺣﺻل:

Transaction
↓
Check Budget
↓
Send Email
ﻻ
.إﺣﻧﺎ اﺧﺗرﻧﺎ:

Daily Scheduled Checkﯾﻌﻧﻲ Hangfire ﻛل ﯾوم ﯾﻌﻣل:

CheckBudgetsJob
19. Budget Check Job
Get all active users
↓
For each user
↓
Get current month's budgets
↓
Get current month's expenses
↓
Group expenses by category
↓
Compare Spent vs Budget
↓
Send Email if threshold exceededاﻟـ Job ﯾﻌﻣل ﺗﻘرﯾﺑًﺎ:
ﻣﺛﻼً:

Food Budget = 2500
Transactions:
500
700
600
500
Total = 2300إذن:

2300 / 2500 = 92%
21. Reports
Pie Chart
Bar Chart
Line Chart
.Alert ﯾﺑﻌتاﻟـ Reports ھﻲ APIs ﺗرﺟﻊ ﺑﯾﺎﻧﺎت ﻣﺎﻟﯾﺔ ﻣﻧظﻣﺔ.
واﻟـ Backend ﻣش ﻣﺳؤول ﻋن اﻟرﺳم.

.JSON ﯾرﺟﻊ Backend ﯾﻌﻧﻲواﻟـ Frontend ﺑﻌد ﻛده ﯾﻌﻣل:

22. Monthly Summaryﻣﺛﻼً:

GET /api/reports/monthly-summary?year=2026&month=9
{
}
"year": 2026,
"month": 9,
"totalIncome": 17000,
"totalExpenses": 8000,
"net": 9000ﯾرﺟﻊ:

.Frontend وده ﻛﻔﺎﯾﺔ ﻟﻠـ
23. Category Breakdownﻣﺛﻼً:

GET /api/reports/category-breakdown?year=2026&month=9
ﯾرﺟﻊ
:
[
]
{
},
{
},
{
}
"category": "Food",
"amount": 2000
"category": "Rent",
"amount": 5000
"category": "Transportation",
"amount": 1000
Pie Chartاﻟـ Frontend ﯾﻘدر ﯾﺣوﻟﮭﺎ ﻟـ:

24. Month-to-Month Comparison
[
{
},
{
},
{
"year": 2026,
"month": 7,
"income": 15000,
"expenses": 7000,
"net": 8000
"year": 2026,
"month": 8,
"income": 16000,
"expenses": 7500,
"net": 8500دي ﻋﺷﺎن ﻧﻘﺎرن اﻟﺷﮭور.
ﻣﺛﻼً:

"year": 2026,
"month": 9,
"income": 17000,
"expenses": 8000,
"net": 9000
}
]
┌──────────────┐
│     
│
└──────┬───────┘
│
┌────────────────┼─────────────────┐
│                
▼                
Categories      
│                
│                
│         
│         
│      
│                     
│                     
│                 
│                                   
┌──────┴──────┐          
│             
Manual       
.Line Chart ﻣﻣﻛن ﯾرﺳم Frontend اﻟـ31. اﻟﺻورة اﻟﻛﺎﻣﻠﺔ ﻟﻠﻣﺷروع
ﻟو ﺟﻣﻌﻧﺎ ﻛل اﻟﻛﻼم اﻟﺳﺎﺑﻖ:

User     
│                 
▼                 
Transactions        
│                 
│                 
Hangfire          
│
▼
Budgets
│
│
│
│          
Recurring      
│             
▼             
│
│
│
│
│
│
└─────────────────┬─────────────────┘
│
▼
Budget Check Job
│
▼
Notification
│
▼
Email
