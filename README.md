# Fixer.IO.Wrapper
 Fixer.io
Programming task 
On https://fixer.io/ you can sign up to get access to an WebApi that provides you with currency  exchange rates. The API provides the rates in JSON structures.  
Sign up to get access and familiarize you with the API. 
1. Below is a few programming task that utilize this service in different ways. a. Create a console application that take two currency codes and one amount as input. The  amount is in the first currency. The program should calculate the currency amount for the  second currency code (using the latest exchange rates). The program should do the  calculation in process and not utilize any external calculation api. 
b. Extend the program with an optional input date, do the same calculation as in step 1 but use  the currency rate for the date inputted to the program. You need to find out the url for  retrieving exchange rate a given date using the documentation on fixer.io. 
c. Create a program that will be executed once a day. The program should retrieve the latest  exchange rate and store it in a database. Using SqlServer and Entity framework is preferred  but not mandatory. This task involves design a suitable database structure. 
2. Optional tasks (time permitting) (Pending)
a. Create a WebAPI (Rest/Json) that offers the same calculations as in 1a and 1b but as a  service. 
b. Build a simple web on top of the service you created in 2b that offers this calculation service  to the user. 
c. Extend the WebAPI from 2a with a new method to return all the exchange rates for one  currency for a given period of time. The exchange rates should be extracted from the  database created in 1c. 
d. Build a web page on top of 2c to show the how an exchange rate develop for a time period,  find a satiable graphical component to visualize the exchange rate. 
The program should be develop in Visual Studio, using C#/.NET for the web pages you can choose  native html/java scrip or a framework you are familiar with
