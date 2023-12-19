using System.Data;
using Dapper;

namespace TimeSeriesPoc;

public class DatabaseInit : BackgroundService
{
    private readonly IDbConnection _connection;

    public DatabaseInit(IDbConnection connection)
    {
        _connection = connection;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _connection.ExecuteAsync("""
                                       CREATE TABLE IF NOT EXISTS sample (
                                         chargerId VARCHAR(36) NOT NULL,
                                         connectorid INT NOT NULL,
                                         transactionid INT NULL,
                                         timestamp TIMESTAMPTZ NOT NULL,
                                         value VARCHAR(50) NOT NULL,
                                         context VARCHAR(50) NOT NULL,
                                         format VARCHAR(50) NOT NULL,
                                         measurand VARCHAR(50) NOT NULL,
                                         phase VARCHAR(5) NULL,
                                         location VARCHAR(50) NOT NULL,
                                         unit VARCHAR(10) NULL
                                       );
                                       SELECT create_hypertable( 'sample', 'timestamp') WHERE not exists(SELECT 1 FROM timescaledb_information.hypertables WHERE hypertable_name = 'sample');
                                       CREATE INDEX IF NOT EXISTS ix_sample_charger_transaction ON sample (chargerId, transactionid, transactionid, measurand, timestamp DESC);
                                       """, stoppingToken);
    }
}