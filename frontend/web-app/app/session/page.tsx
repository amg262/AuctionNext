import getSession from "@/app/actions/authActions";
import Heading from "@/app/components/Heading";
import AuthTest from "@/app/session/AuthTest";

export default async function Session() {
  const session = await getSession();
  return (
      <div>
        <Heading title={"Session Dashboard"}></Heading>
        <div className='bg-blue-200 border-2 border-blue-500'>
          <h3 className={"text-lg"}>Session</h3>
          <pre>{JSON.stringify(session, null, 2)}</pre>
        </div>
        <div className='mt-4'>
          <AuthTest/>
        </div>
      </div>
  );
}