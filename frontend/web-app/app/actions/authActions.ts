import {getServerSession} from "next-auth";
import {authOptions} from "@/app/api/auth/[...nextauth]/route";

export default async function getSession() {
  return await getServerSession(authOptions);
}

export async function getCurrentUser() {
  try {
    const session = await getSession();
    console.log('session', session);

    if (!session) return null;

    return session.user;
  } catch (e) {
    console.error(e);
    return null;
  }
}
