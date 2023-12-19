# Documentation
* https://khalidabuhakmeh.com/getting-started-with-ef-core-postgresql-and-timescaledb  
* https://docs.timescale.com/getting-started/latest/

# What you can do
* Create a sample table as hypertable
* Import meter values from OpenSearch. You need to provide the charge point id and the transaction id
* Visualize in [grafana](http://localhost:3000)

# Example of test data
| Charge point id | Transaction id | Interval                                            |
| --------------- | -------------- |---------------------------------------------------- |
| 11172           | 470            | "2023-12-15 19:06:28+00" - "2023-12-16 06:29:51+00" |
| 11172           | 472            | "2023-12-18 07:15:38+00" - "2023-12-18 11:47:45+00" |
| 15910           | 583            | "2023-12-19 06:12:50+00" - "2023-12-19 11:03:20+00" |

![](/TimeSeriesPoc/assets/1.png)
![](/TimeSeriesPoc/assets/2.png)
![](/TimeSeriesPoc/assets/3.png)