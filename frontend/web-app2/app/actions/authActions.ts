import {getServerSession} from "next-auth";
import {authOptions} from "@/app/api/auth/[...nextauth]/route";
import {cookies, headers} from "next/headers";
import {NextApiRequest} from "next";
import {getToken} from "next-auth/jwt";

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

export async function getTokenWorkaround() {
  const req = {
    headers: Object.fromEntries(headers() as Headers),
    cookies: Object.fromEntries(
        cookies()
            .getAll()
            .map(c => [c.name, c.value])
    )
  } as NextApiRequest;

  return await getToken({req});
}