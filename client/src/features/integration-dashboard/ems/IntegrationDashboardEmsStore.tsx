import { IntegrationDashboardStoreInterface } from "../IntegrationDashboardStoreInterface";
import { observable } from "mobx";
import { IntegrationDashboardEmsRunIntegration } from "./run/IntegrationDashboardEmsRunIntegration";
import { IntegrationTypeEnum } from "clients/deal-system-client-definitions";

export default class IntegrationDashboardEmsStore implements IntegrationDashboardStoreInterface {
    description = "EMS Integration"
    integrationType = IntegrationTypeEnum.EmsTradepoint
    runIntegrationComponent = IntegrationDashboardEmsRunIntegration
}