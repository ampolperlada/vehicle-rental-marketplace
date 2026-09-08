import OwnerAssets from "./Owner/ownerAssets";
import CustomerAssets from "./Customer/customerAssets";
import { isOwner } from "@/modules/auth/_components/api/authApi";

const Assets = () => {
  if (isOwner) {
    return <OwnerAssets />;
  }

  return <CustomerAssets />;
};

export default Assets;
