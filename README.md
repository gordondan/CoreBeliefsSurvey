---
description: A small application to look at Blazor, Telerik and Azure web services.
---

# 🌳 Core Beliefs Survey

{% hint style="info" %}
**Dan Gordon July 18, 2023**
{% endhint %}

## Overview

&#x20;A core belief is a fundamental view, understanding, or conviction that a person holds regarding themselves, others, or the world around them. These beliefs often originate from past experiences, cultural or societal influences, or personal reflections, and they shape our perception of reality, inform our behavior, and guide our decision-making processes.

Core beliefs can be positive or negative. Positive core beliefs might include things like "I am capable" or "People are generally good", which promote self-confidence and trust. Negative core beliefs, on the other hand, can undermine self-esteem and hinder interpersonal relationships. Examples might include beliefs like "I am unworthy" or "People cannot be trusted".

I got an assignment from my therapist to take a look at my core beliefs and an assignment from Doug to learn more about Blazor, Azure and the Telerik components or rather it may be more fair to say I asked for an assignment from Doug to look at those things and he didn’t say no.

A tool that would allow a person to take a survey regarding their core beliefs and answer on a Likert scale was a good meld of the two requirements. In this project I hard coded the question structure, but if there were extension to the project it would be straightforward to generalize the structure. In as much as the Qualtrics license has significantly increased in price there may be an academic desire for a light weight high functioning custom survey tool. That is where I would expect the work in this project to get its most likely continued life outside of simply an academic exercise if that were to happen.

At one point in the project I used Azure tables to hold responses and used an API call to retrieve them, but later simplified that part of the code to use local storage to improve the predictability of data grid from the Telerik library. It seemed like it wasn’t rendering every other time and I had the sense there was a race condition, but didn’t spend time trying to debug that in as much as the core functionality I was look for would not include survey result storage and analysis, but again something straight forward to add on later if it is wanted.

A list of 120 core beliefs, 60 positive and 60 negative are stored in an azure table with a single partition key and unique row keys. The beliefs are all pulled from the table on page load. A user is prompted whether they want to answer 5,10,25,50 or all questions. A random selection is taken from the beliefs table according to the number selected and the questions are presented through a Blazor component one at a time. There is a progress tracker that lets the user know how many questions they have answered and another indicator that lets them know which question they are on (x of y). After a user answers all of the questions or clicks on submit a Telerik data grid is displayed with a color coded report of the survey results and icons reflecting the positivity of the results. I also used Telerik libraries to implement a view pdf feature. When the view pdf button is clicked a pdf file is written from scratch using the Telerik library, the file contents are written to local storage and navigation is moved to a page that uses the Telerik pdf view component. The data grid also includes and export to Excel functionality out of the box.

I started writing the documentation in Word and then tried pulling my hair out, but there wasn't any handy on the top of my head, so I decided to try gitbook.com. So far it looks promising. It looks like licensing runs about $240/month ([https://www.gitbook.com/pricing](https://www.gitbook.com/pricing)) so it isn't likely a good fit for the team yet.



{% content-ref url="technology/methodology.md" %}
[methodology.md](technology/methodology.md)
{% endcontent-ref %}
