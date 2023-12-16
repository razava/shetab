using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class TablesRenamed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActorProcessStage_Stage_StagesId",
                table: "ActorProcessStage");

            migrationBuilder.DropForeignKey(
                name: "FK_Answer_AspNetUsers_UserId",
                table: "Answer");

            migrationBuilder.DropForeignKey(
                name: "FK_Answer_Poll_PollId",
                table: "Answer");

            migrationBuilder.DropForeignKey(
                name: "FK_BotActors_Transition_TransitionId",
                table: "BotActors");

            migrationBuilder.DropForeignKey(
                name: "FK_Choice_Poll_PollId",
                table: "Choice");

            migrationBuilder.DropForeignKey(
                name: "FK_PollAnswerPollChoice_Answer_AnswersId",
                table: "PollAnswerPollChoice");

            migrationBuilder.DropForeignKey(
                name: "FK_PollAnswerPollChoice_Choice_ChoicesId",
                table: "PollAnswerPollChoice");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessReasonProcessTransition_Reason_ReasonListId",
                table: "ProcessReasonProcessTransition");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessReasonProcessTransition_Transition_TransitionsId",
                table: "ProcessReasonProcessTransition");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Reason_LastReasonId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Stage_CurrentStageId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Transition_LastTransitionId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Stage_AspNetRoles_DisplayRoleId",
                table: "Stage");

            migrationBuilder.DropForeignKey(
                name: "FK_Stage_Process_ProcessId",
                table: "Stage");

            migrationBuilder.DropForeignKey(
                name: "FK_Transition_Process_ProcessId",
                table: "Transition");

            migrationBuilder.DropForeignKey(
                name: "FK_Transition_Stage_FromId",
                table: "Transition");

            migrationBuilder.DropForeignKey(
                name: "FK_Transition_Stage_ToId",
                table: "Transition");

            migrationBuilder.DropForeignKey(
                name: "FK_TransitionLogs_Reason_ReasonId",
                table: "TransitionLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_TransitionLogs_Transition_TransitionId",
                table: "TransitionLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transition",
                table: "Transition");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Stage",
                table: "Stage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reason",
                table: "Reason");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Choice",
                table: "Choice");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Answer",
                table: "Answer");

            migrationBuilder.RenameTable(
                name: "Transition",
                newName: "ProcessTransition");

            migrationBuilder.RenameTable(
                name: "Stage",
                newName: "ProcessStage");

            migrationBuilder.RenameTable(
                name: "Reason",
                newName: "ProcessReason");

            migrationBuilder.RenameTable(
                name: "Choice",
                newName: "PollChoice");

            migrationBuilder.RenameTable(
                name: "Answer",
                newName: "PollAnswer");

            migrationBuilder.RenameIndex(
                name: "IX_Transition_ToId",
                table: "ProcessTransition",
                newName: "IX_ProcessTransition_ToId");

            migrationBuilder.RenameIndex(
                name: "IX_Transition_ProcessId",
                table: "ProcessTransition",
                newName: "IX_ProcessTransition_ProcessId");

            migrationBuilder.RenameIndex(
                name: "IX_Transition_FromId",
                table: "ProcessTransition",
                newName: "IX_ProcessTransition_FromId");

            migrationBuilder.RenameIndex(
                name: "IX_Stage_ProcessId",
                table: "ProcessStage",
                newName: "IX_ProcessStage_ProcessId");

            migrationBuilder.RenameIndex(
                name: "IX_Stage_DisplayRoleId",
                table: "ProcessStage",
                newName: "IX_ProcessStage_DisplayRoleId");

            migrationBuilder.RenameIndex(
                name: "IX_Choice_PollId",
                table: "PollChoice",
                newName: "IX_PollChoice_PollId");

            migrationBuilder.RenameIndex(
                name: "IX_Answer_UserId",
                table: "PollAnswer",
                newName: "IX_PollAnswer_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Answer_PollId",
                table: "PollAnswer",
                newName: "IX_PollAnswer_PollId");

            migrationBuilder.AlterColumn<int>(
                name: "PollId",
                table: "PollAnswer",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProcessTransition",
                table: "ProcessTransition",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProcessStage",
                table: "ProcessStage",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProcessReason",
                table: "ProcessReason",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PollChoice",
                table: "PollChoice",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PollAnswer",
                table: "PollAnswer",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ActorProcessStage_ProcessStage_StagesId",
                table: "ActorProcessStage",
                column: "StagesId",
                principalTable: "ProcessStage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BotActors_ProcessTransition_TransitionId",
                table: "BotActors",
                column: "TransitionId",
                principalTable: "ProcessTransition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PollAnswer_AspNetUsers_UserId",
                table: "PollAnswer",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PollAnswer_Poll_PollId",
                table: "PollAnswer",
                column: "PollId",
                principalTable: "Poll",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PollAnswerPollChoice_PollAnswer_AnswersId",
                table: "PollAnswerPollChoice",
                column: "AnswersId",
                principalTable: "PollAnswer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PollAnswerPollChoice_PollChoice_ChoicesId",
                table: "PollAnswerPollChoice",
                column: "ChoicesId",
                principalTable: "PollChoice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PollChoice_Poll_PollId",
                table: "PollChoice",
                column: "PollId",
                principalTable: "Poll",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessReasonProcessTransition_ProcessReason_ReasonListId",
                table: "ProcessReasonProcessTransition",
                column: "ReasonListId",
                principalTable: "ProcessReason",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessReasonProcessTransition_ProcessTransition_TransitionsId",
                table: "ProcessReasonProcessTransition",
                column: "TransitionsId",
                principalTable: "ProcessTransition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessStage_AspNetRoles_DisplayRoleId",
                table: "ProcessStage",
                column: "DisplayRoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessStage_Process_ProcessId",
                table: "ProcessStage",
                column: "ProcessId",
                principalTable: "Process",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessTransition_ProcessStage_FromId",
                table: "ProcessTransition",
                column: "FromId",
                principalTable: "ProcessStage",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessTransition_ProcessStage_ToId",
                table: "ProcessTransition",
                column: "ToId",
                principalTable: "ProcessStage",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessTransition_Process_ProcessId",
                table: "ProcessTransition",
                column: "ProcessId",
                principalTable: "Process",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_ProcessReason_LastReasonId",
                table: "Reports",
                column: "LastReasonId",
                principalTable: "ProcessReason",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_ProcessStage_CurrentStageId",
                table: "Reports",
                column: "CurrentStageId",
                principalTable: "ProcessStage",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_ProcessTransition_LastTransitionId",
                table: "Reports",
                column: "LastTransitionId",
                principalTable: "ProcessTransition",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TransitionLogs_ProcessReason_ReasonId",
                table: "TransitionLogs",
                column: "ReasonId",
                principalTable: "ProcessReason",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TransitionLogs_ProcessTransition_TransitionId",
                table: "TransitionLogs",
                column: "TransitionId",
                principalTable: "ProcessTransition",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActorProcessStage_ProcessStage_StagesId",
                table: "ActorProcessStage");

            migrationBuilder.DropForeignKey(
                name: "FK_BotActors_ProcessTransition_TransitionId",
                table: "BotActors");

            migrationBuilder.DropForeignKey(
                name: "FK_PollAnswer_AspNetUsers_UserId",
                table: "PollAnswer");

            migrationBuilder.DropForeignKey(
                name: "FK_PollAnswer_Poll_PollId",
                table: "PollAnswer");

            migrationBuilder.DropForeignKey(
                name: "FK_PollAnswerPollChoice_PollAnswer_AnswersId",
                table: "PollAnswerPollChoice");

            migrationBuilder.DropForeignKey(
                name: "FK_PollAnswerPollChoice_PollChoice_ChoicesId",
                table: "PollAnswerPollChoice");

            migrationBuilder.DropForeignKey(
                name: "FK_PollChoice_Poll_PollId",
                table: "PollChoice");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessReasonProcessTransition_ProcessReason_ReasonListId",
                table: "ProcessReasonProcessTransition");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessReasonProcessTransition_ProcessTransition_TransitionsId",
                table: "ProcessReasonProcessTransition");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessStage_AspNetRoles_DisplayRoleId",
                table: "ProcessStage");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessStage_Process_ProcessId",
                table: "ProcessStage");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessTransition_ProcessStage_FromId",
                table: "ProcessTransition");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessTransition_ProcessStage_ToId",
                table: "ProcessTransition");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessTransition_Process_ProcessId",
                table: "ProcessTransition");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_ProcessReason_LastReasonId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_ProcessStage_CurrentStageId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_ProcessTransition_LastTransitionId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_TransitionLogs_ProcessReason_ReasonId",
                table: "TransitionLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_TransitionLogs_ProcessTransition_TransitionId",
                table: "TransitionLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProcessTransition",
                table: "ProcessTransition");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProcessStage",
                table: "ProcessStage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProcessReason",
                table: "ProcessReason");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PollChoice",
                table: "PollChoice");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PollAnswer",
                table: "PollAnswer");

            migrationBuilder.RenameTable(
                name: "ProcessTransition",
                newName: "Transition");

            migrationBuilder.RenameTable(
                name: "ProcessStage",
                newName: "Stage");

            migrationBuilder.RenameTable(
                name: "ProcessReason",
                newName: "Reason");

            migrationBuilder.RenameTable(
                name: "PollChoice",
                newName: "Choice");

            migrationBuilder.RenameTable(
                name: "PollAnswer",
                newName: "Answer");

            migrationBuilder.RenameIndex(
                name: "IX_ProcessTransition_ToId",
                table: "Transition",
                newName: "IX_Transition_ToId");

            migrationBuilder.RenameIndex(
                name: "IX_ProcessTransition_ProcessId",
                table: "Transition",
                newName: "IX_Transition_ProcessId");

            migrationBuilder.RenameIndex(
                name: "IX_ProcessTransition_FromId",
                table: "Transition",
                newName: "IX_Transition_FromId");

            migrationBuilder.RenameIndex(
                name: "IX_ProcessStage_ProcessId",
                table: "Stage",
                newName: "IX_Stage_ProcessId");

            migrationBuilder.RenameIndex(
                name: "IX_ProcessStage_DisplayRoleId",
                table: "Stage",
                newName: "IX_Stage_DisplayRoleId");

            migrationBuilder.RenameIndex(
                name: "IX_PollChoice_PollId",
                table: "Choice",
                newName: "IX_Choice_PollId");

            migrationBuilder.RenameIndex(
                name: "IX_PollAnswer_UserId",
                table: "Answer",
                newName: "IX_Answer_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_PollAnswer_PollId",
                table: "Answer",
                newName: "IX_Answer_PollId");

            migrationBuilder.AlterColumn<int>(
                name: "PollId",
                table: "Answer",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transition",
                table: "Transition",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Stage",
                table: "Stage",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reason",
                table: "Reason",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Choice",
                table: "Choice",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Answer",
                table: "Answer",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ActorProcessStage_Stage_StagesId",
                table: "ActorProcessStage",
                column: "StagesId",
                principalTable: "Stage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Answer_AspNetUsers_UserId",
                table: "Answer",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Answer_Poll_PollId",
                table: "Answer",
                column: "PollId",
                principalTable: "Poll",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BotActors_Transition_TransitionId",
                table: "BotActors",
                column: "TransitionId",
                principalTable: "Transition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Choice_Poll_PollId",
                table: "Choice",
                column: "PollId",
                principalTable: "Poll",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PollAnswerPollChoice_Answer_AnswersId",
                table: "PollAnswerPollChoice",
                column: "AnswersId",
                principalTable: "Answer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PollAnswerPollChoice_Choice_ChoicesId",
                table: "PollAnswerPollChoice",
                column: "ChoicesId",
                principalTable: "Choice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessReasonProcessTransition_Reason_ReasonListId",
                table: "ProcessReasonProcessTransition",
                column: "ReasonListId",
                principalTable: "Reason",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessReasonProcessTransition_Transition_TransitionsId",
                table: "ProcessReasonProcessTransition",
                column: "TransitionsId",
                principalTable: "Transition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Reason_LastReasonId",
                table: "Reports",
                column: "LastReasonId",
                principalTable: "Reason",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Stage_CurrentStageId",
                table: "Reports",
                column: "CurrentStageId",
                principalTable: "Stage",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Transition_LastTransitionId",
                table: "Reports",
                column: "LastTransitionId",
                principalTable: "Transition",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Stage_AspNetRoles_DisplayRoleId",
                table: "Stage",
                column: "DisplayRoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stage_Process_ProcessId",
                table: "Stage",
                column: "ProcessId",
                principalTable: "Process",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transition_Process_ProcessId",
                table: "Transition",
                column: "ProcessId",
                principalTable: "Process",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transition_Stage_FromId",
                table: "Transition",
                column: "FromId",
                principalTable: "Stage",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transition_Stage_ToId",
                table: "Transition",
                column: "ToId",
                principalTable: "Stage",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TransitionLogs_Reason_ReasonId",
                table: "TransitionLogs",
                column: "ReasonId",
                principalTable: "Reason",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TransitionLogs_Transition_TransitionId",
                table: "TransitionLogs",
                column: "TransitionId",
                principalTable: "Transition",
                principalColumn: "Id");
        }
    }
}
