#MintnInovationTestTask

##Case

A job of 1000 images is going to be edited by a crew of 3 people:
- P1: 1 image per 2 minutes
- P2: 1 image per 3 minutes
- P3: 1 image per 4 minutes

How long will this job take in total?
How many images will be edited by every person?

Scoring:
- Correct answer and explain the necessary steps 
- Working code
- It should work for any situation (amount of images, amount of people, individual speed)

Please focus on:
- OOP implementation 
- N-tier architecture implementation
- Unit testing implementation 
- General programing rules and standards usage.

##How to run
###Preparing
```bash
git clone https://github.com/Toolf/MintnInovationTestTask.git
cd MintnInovationTestTask
dotnet restore
```
###Start server
```bash
cd Dissolve.Server
dotnet run
```

### Start client

```bash
cd Dissolve.Client
dotnet run
```

Solution for case:
- P - people count.
- i = 1..P - index of person.
- Mi - minutes fot edit one image by i-th person.
- Fi - images by 1 minute edit by i-th person.
1. Calculate F = sum(Fi), i=1...P.
2. Minimum time for edit all pages Tmin = (image count) / F.
3. Round Tmin by the biggest Mi to the largest and save in Tmax.
4. Calculate how many images peoples can edit by time Tmax and save in TotalImageEdited.
5. If TotalImageEdited more them image count, sub the biggest Mi from Tmax.
6. Calculate how many images the slowest person edit by Tmax and save this amount to ImageEdited.
7. Reduce image count by ImageEdited.
8. Remove the slowest person
9. If people exist go to the step (1)

Concept of implementation

![alt text](https://github.com/Toolf/MintnInovationTestTask/blob/media/images/architecture.png)
