using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvitationLinks_Organizations_OrganizationId",
                table: "InvitationLinks");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "InvitationLinks",
                newName: "DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_InvitationLinks_OrganizationId",
                table: "InvitationLinks",
                newName: "IX_InvitationLinks_DepartmentId");

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationModelId",
                table: "InvitationLinks",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvitationLinks_OrganizationModelId",
                table: "InvitationLinks",
                column: "OrganizationModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvitationLinks_Departments_DepartmentId",
                table: "InvitationLinks",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InvitationLinks_Organizations_OrganizationModelId",
                table: "InvitationLinks",
                column: "OrganizationModelId",
                principalTable: "Organizations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvitationLinks_Departments_DepartmentId",
                table: "InvitationLinks");

            migrationBuilder.DropForeignKey(
                name: "FK_InvitationLinks_Organizations_OrganizationModelId",
                table: "InvitationLinks");

            migrationBuilder.DropIndex(
                name: "IX_InvitationLinks_OrganizationModelId",
                table: "InvitationLinks");

            migrationBuilder.DropColumn(
                name: "OrganizationModelId",
                table: "InvitationLinks");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                table: "InvitationLinks",
                newName: "OrganizationId");

            migrationBuilder.RenameIndex(
                name: "IX_InvitationLinks_DepartmentId",
                table: "InvitationLinks",
                newName: "IX_InvitationLinks_OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvitationLinks_Organizations_OrganizationId",
                table: "InvitationLinks",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
