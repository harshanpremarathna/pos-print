# POS Print

### C# windows application with minimum code lines to print values to POS printers using RAW data and OPOS library

[![Build Status](https://travis-ci.org/joemccann/dillinger.svg?branch=master)](https://travis-ci.org/joemccann/dillinger)

> Note: This doesn't include best practices and niether intention to follow the best practices. But intention was to provide a sample code which can be extent to communicate with POS printers. 

## Features

- Print bill using
-- Microsoft POS for .NET
-- Without any library and using RAW data
- Print barcode
- Cut paper

> Note: I'm trying to add more details in future with better guideline. But if you feel to update the code, please no hesitate. 

This project include two type of methods have been used to print the bill. So you can refer either approach. But when using Microsoft POS library, it contains additional steps to do in the client PC like install printer's vendor specific OPOS objects etc.

So to use Microsoft POS library, you should install below in both development and client PC.
Microsoft Point of Service for .NET v1.14 (POS for .NET)
Download from https://www.microsoft.com/en-us/download/details.aspx?id=42081

And refer this from the both projects in the sample application. But if you check the code bit more, you will find, it's enough to use the reference from the MyApp.POSPrint.Print project, if you can do the small refactors to the code.

But if your intention to use the RAW method, just remove the "Microsoft.PointOfService" reference from both project and delete the "OPOSPrinter" class and related method call from the "PrintForm".
