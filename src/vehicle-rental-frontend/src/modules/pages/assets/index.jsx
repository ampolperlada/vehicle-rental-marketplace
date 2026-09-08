import { getUser } from "@/modules/auth/_components/api/authApi";
import OwnerAssets from "./Owner/ownerAssets";
import CustomerAssets from "./Customer/customerAssets";

const Assets = () => {
  const user = getUser();
  const isAdmin = user?.role === "Admin";

  if (isAdmin) {
    return <OwnerAssets />;
  }

  return <CustomerAssets />;
};

export default Assets;
