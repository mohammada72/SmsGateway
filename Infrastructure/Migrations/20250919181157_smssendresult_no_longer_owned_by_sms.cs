using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class smssendresult_no_longer_owned_by_sms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SmsSendResult",
                table: "SmsSendResult");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SmsSendResult",
                table: "SmsSendResult",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_SmsSendResult_SmsId",
                table: "SmsSendResult",
                column: "SmsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SmsSendResult",
                table: "SmsSendResult");

            migrationBuilder.DropIndex(
                name: "IX_SmsSendResult_SmsId",
                table: "SmsSendResult");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SmsSendResult",
                table: "SmsSendResult",
                columns: new[] { "SmsId", "Id" });
        }
    }
}
