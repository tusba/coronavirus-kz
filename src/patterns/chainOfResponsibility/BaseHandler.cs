namespace Tusba.Patterns.ChainOfResponsibility
{
	public abstract class BaseHandler<Q, R> : InterfaceHandler<Q, R>
	{
		public BaseHandler<Q, R>? Next { get; private init; }

		protected virtual R? DefaultResult => default(R);

		public BaseHandler(BaseHandler<Q, R>? next = null) => Next = next;

		public R? handle(Q query)
		{
			if (Next is not null)
			{
				return Next.handle(query);
			}

			return DefaultResult;
		}
	}
}
